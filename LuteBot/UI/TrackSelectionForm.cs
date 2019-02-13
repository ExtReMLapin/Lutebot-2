﻿using LuteBot.Logger;
using LuteBot.TrackSelection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuteBot.UI
{
    public partial class TrackSelectionForm : Form
    {
        TrackSelectionManager trackSelectionManager;
        private ConfigManager configManager;

        public TrackSelectionForm(TrackSelectionManager trackSelectionManager, ConfigManager configManager)
        {
            this.trackSelectionManager = trackSelectionManager;
            trackSelectionManager.TrackChanged += new EventHandler(TrackChangedHandler);
            InitializeComponent();
            InitLists();
            trackSelectionManager.autoLoadProfile = AutoActivateCheckBox.Checked;
            this.configManager = configManager;
            GlobalLogger.Log("TrackSelectionForm", LoggerManager.LoggerLevel.Essential, "TrackSelectionForm Initialised");
        }

        private void InitLists()
        {
            TrackListBox.Items.Clear();
            ChannelsListBox.Items.Clear();
            foreach (MidiChannelItem channel in trackSelectionManager.MidiChannels)
            {
                ChannelsListBox.Items.Add(channel.Name , channel.Active);
            }
            foreach (TrackItem track in trackSelectionManager.MidiTracks)
            {
                TrackListBox.Items.Add(track.Name, track.Active);
            }
            Refresh();
        }

        private void TrackChangedHandler(object sender, EventArgs e)
        {
            InitLists();
        }

        private void TrackSelectionForm_Closing(object sender, FormClosingEventArgs e)
        {
            configManager.SetWindowCoordinates("TrackSelectionPos", new Point() { X = this.Top, Y = this.Left });
            configManager.Save();
        }

        private void SelectAllChannelsTextBox_CheckedChanged(object sender, EventArgs e)
        {
            trackSelectionManager.ActivateAllChannels = SelectAllChannelsCheckBox.Checked;
            ChannelsListBox.Enabled = !SelectAllChannelsCheckBox.Checked;
        }

        private void ChannelListBox_ItemChecked(object sender, ItemCheckEventArgs e)
        {
            trackSelectionManager.ToggleChannelActivation(!(e.CurrentValue == CheckState.Checked), e.Index);
        }

        private void SongProfileSaveButton_Click(object sender, EventArgs e)
        {
            if (trackSelectionManager.FileName != null)
            {
                trackSelectionManager.SaveTrackManager();
            }
        }

        private void LoadProfileButton_Click(object sender, EventArgs e)
        {
            if (trackSelectionManager.FileName != null)
            {
                trackSelectionManager.LoadTrackManager();
                InitLists();
            }
        }

        private void AutoActivateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            trackSelectionManager.autoLoadProfile = AutoActivateCheckBox.Checked;
        }

        private void SelectAllTracksCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            trackSelectionManager.ActivateAllChannels = SelectAllTracksCheckBox.Checked;
            TrackListBox.Enabled = !SelectAllTracksCheckBox.Checked;
        }

        private void TrackListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            trackSelectionManager.ToggleTrackActivation(!(e.CurrentValue == CheckState.Checked), e.Index);
        }
    }
}
