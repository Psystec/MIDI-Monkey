using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MIDI_Monkey.Services
{
    public class MidiChannelService
    {
        public Dictionary<int, bool> EnabledChannels { get; set; }
        public MidiChannelService()
        {
            EnabledChannels = new Dictionary<int, bool>();

            // Enable all 16 MIDI channels by default
            for (int i = 1; i <= 16; i++)
            {
                EnabledChannels[i] = true;
            }
        }

        /// <summary>
        /// Check if a specific channel is enabled
        /// </summary>
        public bool IsChannelEnabled(int channel)
        {
            if (channel < 1 || channel > 16)
                return false;

            return EnabledChannels.ContainsKey(channel) && EnabledChannels[channel];
        }

        /// <summary>
        /// Enable or disable a channel
        /// </summary>
        public void SetChannel(int channel, bool enabled)
        {
            if (channel >= 1 && channel <= 16)
            {
                EnabledChannels[channel] = enabled;
            }
        }

        /// <summary>
        /// Enable all channels
        /// </summary>
        public void EnableAllChannels()
        {
            for (int i = 1; i <= 16; i++)
            {
                EnabledChannels[i] = true;
            }
        }

        /// <summary>
        /// Disable all channels
        /// </summary>
        public void DisableAllChannels()
        {
            for (int i = 1; i <= 16; i++)
            {
                EnabledChannels[i] = false;
            }
        }
    }
}
