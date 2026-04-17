using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class SoundsSettingsMenu : TextureRect
	{
        [Export] private Button backToTitleButton;

        [Export] public HSlider overallVolumeSettings;
        [Export] private HSlider SoundsVolumeSettings;
        [Export] private HSlider MusicsVolumeSettings;

        public override void _Ready()
		{
            backToTitleButton.Pressed += BackToTitle;

            overallVolumeSettings.ValueChanged += UpdateOverallVolume;
            SoundsVolumeSettings.ValueChanged += UpdateSoundsVolume;
            MusicsVolumeSettings.ValueChanged += UpdateMusicsVolume;

            overallVolumeSettings.Value = AudioServer.GetBusVolumeDb(0);
            SoundsVolumeSettings.Value = AudioServer.GetBusVolumeDb(2);
            MusicsVolumeSettings.Value = AudioServer.GetBusVolumeDb(1);
            //overallVolumeSettings.Value = SoundManager.GetInstance().overallVolume;
            //SoundsVolumeSettings.Value = SoundManager.GetInstance().soundsVolume;
            //MusicsVolumeSettings.Value = SoundManager.GetInstance().musicVolume;

            //GD.Print(AudioServer.GetBusVolumeDb(0));
        }

        private void BackToTitle()
        {
            UIManager.GetInstance().GoToTitle();
            SoundManager.GetInstance().PlayClick();
        }
        private void UpdateOverallVolume(double pValue)
        {
            if (overallVolumeSettings.Value <= overallVolumeSettings.MinValue) AudioServer.SetBusMute(0, true);
            else
            {
                AudioServer.SetBusMute(0, false);
                AudioServer.SetBusVolumeDb(0, (float)pValue);
                
            }

            SoundManager.GetInstance().overallVolume = (float)overallVolumeSettings.Value;
            

        }
        private void UpdateMusicsVolume(double value)
        {
            if (MusicsVolumeSettings.Value <= MusicsVolumeSettings.MinValue) AudioServer.SetBusMute(1, true);
            else
            {
                AudioServer.SetBusVolumeDb(1, (float)value);
                AudioServer.SetBusMute(1, false);
            }

            SoundManager.GetInstance().musicVolume = (float)MusicsVolumeSettings.Value;
        }
        private void UpdateSoundsVolume(double value)
        {
            if (SoundsVolumeSettings.Value <= SoundsVolumeSettings.MinValue) AudioServer.SetBusMute(2, true);
            else
            {
                AudioServer.SetBusVolumeDb(2, (float)value);
                AudioServer.SetBusMute(2, false);
            }

            SoundManager.GetInstance().soundsVolume = (float)SoundsVolumeSettings.Value;
        }
    }
}
