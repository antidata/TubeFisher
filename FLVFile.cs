#region license
/***************************************************************************
 *		youtubeFisher
 *		Copyright(C) 2009-2011 by fremyd
 * 
 *		This program is free software; you can redistribute it and/or 
 *		modify it under the terms of the GNU General Public License as
 *		published by the Free Software Foundation; either version 2 of 
 *		the License, or (at your option) any later version.
 *		This program is distributed WITHOUT ANY WARRANTY; without even the
 *		implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 *		See the GNU General Public License for more details.
 *		This class is an hack of a class copyright by J.D.Purcell and 
 *		distribuited under the GNU General Public License. Thanks to him.
 *	
 *****************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public delegate bool OverwriteDelegate(ref string destPath);

public class FLVFile : IDisposable
{
	string _inputPath, _outputDirectory, _outputPathBase;
	OverwriteDelegate setoutput;
	FileStream _fs;
	long _fileOffset, _fileLength;
	IAudioWriter _audioWriter;
	List<uint> _videoTimeStamps;
	bool _extractedAudio;
	List<string> _warnings;

	public FLVFile(string path)
	{
		_inputPath = path;
		_outputDirectory = Path.GetDirectoryName(path);
		_warnings = new List<string>();
		_fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 65536);
		_fileOffset = 0;
		_fileLength = _fs.Length;
	}

	public string OutputDirectory
	{
		get { return _outputDirectory; }
		set { _outputDirectory = value; }
	}

	public List<string> Warnings
	{
		get { return _warnings; }
	}

	public bool ExtractedAudio
	{
		get { return _extractedAudio; }
	}

	public void ExtractStreams(OverwriteDelegate outputDeleg)
	{
		uint dataOffset, flags, prevTagSize;

		_outputPathBase = Path.Combine(_outputDirectory, Path.GetFileNameWithoutExtension(_inputPath));
		this.setoutput = outputDeleg;
		_videoTimeStamps = new List<uint>();

		Seek(0);
        if (ReadUInt32() != 0x464C5601)
        {	                  // not FLV file..
            throw new ArgumentException("Invalid input file. Impossible to extract audio track.");
        }

		flags = ReadUInt8();
		dataOffset = ReadUInt32();

		Seek(dataOffset);

		prevTagSize = ReadUInt32();
		while (_fileOffset < _fileLength)
		{
			if (!ReadTag()) break;
			if ((_fileLength - _fileOffset) < 4) break;
			prevTagSize = ReadUInt32();
		}
		CloseOutput(false);
	}

	private void CloseOutput(bool disposing)
	{
		if (_audioWriter != null)
		{
			_audioWriter.Finish();
			if (disposing && (_audioWriter.Path != null))
			{
				try { 
					File.Delete(_audioWriter.Path); 
				}catch { }
			}
			_audioWriter = null;
		}
	}

	private bool ReadTag()
	{
		uint tagType, dataSize, timeStamp, streamID, mediaInfo;
		byte[] data;

		if ((_fileLength - _fileOffset) < 11)
			return false;

		// Read tag header
		tagType = ReadUInt8();
		dataSize = ReadUInt24();
		timeStamp = ReadUInt24();
		timeStamp |= ReadUInt8() << 24;
		streamID = ReadUInt24();

		// Read tag data
		if (dataSize == 0)
			return true;
		if ((_fileLength - _fileOffset) < dataSize)
			return false;

		mediaInfo = ReadUInt8();
		dataSize -= 1;
		data = ReadBytes((int)dataSize);

		if (tagType == 0x8)
		{  // Audio
			if (_audioWriter == null)
			{
				_audioWriter = GetAudioWriter(mediaInfo);
				_extractedAudio = !(_audioWriter is DummyAudioWriter);
			}
			_audioWriter.WriteChunk(data, timeStamp);
		}
		else if ((tagType == 0x9) && ((mediaInfo >> 4) != 5))
		{ /* Video*/ }

		return true;
	}

	private IAudioWriter GetAudioWriter(uint mediaInfo)
	{
		uint format = mediaInfo >> 4;
		uint rate = (mediaInfo >> 2) & 0x3;
		uint bits = (mediaInfo >> 1) & 0x1;
		uint chans = mediaInfo & 0x1;
		string path;

		if ((format == 2) || (format == 14))
		{ // MP3
			path = _outputPathBase + ".mp3";
			if (!CanWriteTo(ref path)) return new DummyAudioWriter();
			return new MP3Writer(path, _warnings);
		}
/*		else if ((format == 0) || (format == 3))
		{ // PCM (WAVE format)
			int sampleRate = 0;
			switch (rate)
			{
				case 0: sampleRate = 5512; break;
				case 1: sampleRate = 11025; break;
				case 2: sampleRate = 22050; break;
				case 3: sampleRate = 44100; break;
			}
			path = _outputPathBase + ".wav";
			if (!CanWriteTo(ref path)) return new DummyAudioWriter();
			if (format == 0)
			{
				_warnings.Add("PCM byte order unspecified, assuming little endian.");
			}
			return new WAVWriter(path, (bits == 1) ? 16 : 8,
				(chans == 1) ? 2 : 1, sampleRate);
		}
*/		else if (format == 10)
		{ // AAC
			path = _outputPathBase + ".aac";
			if (!CanWriteTo(ref path)) return new DummyAudioWriter();
			return new AACWriter(path);
		}
/*		else if (format == 11)
		{ // Speex
			path = _outputPathBase + ".spx";
			if (!CanWriteTo(ref path)) return new DummyAudioWriter();
			return new SpeexWriter(path, (int)(_fileLength & 0xFFFFFFFF));
		}
*/		else
		{
			string typeStr;

			if (format == 1)
				typeStr = "ADPCM";
			else if ((format == 4) || (format == 5) || (format == 6))
				typeStr = "Nellymoser";
			else
				typeStr = "format=" + format.ToString();

			_warnings.Add("Unable to extract audio (" + typeStr + " is unsupported).");

			return new DummyAudioWriter();
		}
	}

	private bool CanWriteTo(ref string path)
	{
		return setoutput(ref path);
	}

	private void Seek(long offset)
	{
		_fs.Seek(offset, SeekOrigin.Begin);
		_fileOffset = offset;
	}

	private uint ReadUInt8()
	{
		_fileOffset += 1;
		return (uint)_fs.ReadByte();
	}

	private uint ReadUInt24()
	{
		byte[] x = new byte[4];
		_fs.Read(x, 1, 3);
		_fileOffset += 3;
		return BitConverterBE.ToUInt32(x, 0);
	}

	private uint ReadUInt32()
	{
		byte[] x = new byte[4];
		_fs.Read(x, 0, 4);
		_fileOffset += 4;
		return BitConverterBE.ToUInt32(x, 0);
	}

	private byte[] ReadBytes(int length)
	{
		byte[] buff = new byte[length];
		_fs.Read(buff, 0, length);
		_fileOffset += length;
		return buff;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_fs != null)
			{
				_fs.Close();
				_fs = null;
			}
			CloseOutput(true);
		}
	}

}