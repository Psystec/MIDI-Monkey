using NAudio.Midi;
using System;
using System.Collections.Generic;
using MIDI_Monkey.Utilities;

namespace MIDI_Monkey.Core
{
    public class MidiDeviceManager : IDisposable
    {
        private MidiIn? _midiIn;
        private bool _isActive;

        public event EventHandler<MidiInMessageEventArgs>? MessageReceived;
        public event EventHandler<MidiInMessageEventArgs>? ErrorReceived;

        public bool IsActive => _isActive;

        public List<string> GetAvailableDevices()
        {
            var devices = new List<string>();
            int deviceCount = MidiIn.NumberOfDevices;

            for (int i = 0; i < deviceCount; i++)
            {
                devices.Add(MidiIn.DeviceInfo(i).ProductName);
            }

            return devices;
        }

        public bool Connect(int deviceIndex)
        {
            try
            {
                if (_isActive)
                {
                    Disconnect();
                }

                if (deviceIndex < 0 || deviceIndex >= MidiIn.NumberOfDevices)
                {
                    throw new ArgumentOutOfRangeException(nameof(deviceIndex), "Invalid MIDI device index");
                }

                _midiIn = new MidiIn(deviceIndex);
                string deviceName = MidiIn.DeviceInfo(deviceIndex).ProductName;

                _midiIn.MessageReceived += OnMessageReceived;
                _midiIn.ErrorReceived += OnErrorReceived;

                _midiIn.Start();
                _isActive = true;

                Logging.DebugLog($"Connected to MIDI Device: {deviceName}");
                return true;
            }
            catch (Exception ex)
            {
                Logging.DebugLog($"ERROR connecting to MIDI device: {ex.Message}");
                Disconnect();
                return false;
            }
        }

        public void Disconnect()
        {
            if (_midiIn != null)
            {
                try
                {
                    _midiIn.Stop();
                    _midiIn.MessageReceived -= OnMessageReceived;
                    _midiIn.ErrorReceived -= OnErrorReceived;
                    _midiIn.Dispose();
                    _midiIn = null;

                    Logging.DebugLog("MIDI device disconnected");
                }
                catch (Exception ex)
                {
                    Logging.DebugLog($"Error disconnecting MIDI device: {ex.Message}");
                }
                finally
                {
                    _isActive = false;
                }
            }
        }

        private void OnMessageReceived(object? sender, MidiInMessageEventArgs e)
        {
            MessageReceived?.Invoke(sender, e);
        }

        private void OnErrorReceived(object? sender, MidiInMessageEventArgs e)
        {
            ErrorReceived?.Invoke(sender, e);
            Logging.DebugLog($"MIDI Error: {e.RawMessage}");
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
