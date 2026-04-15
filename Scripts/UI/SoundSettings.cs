using Godot;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class SoundSettings : VBoxContainer
	{
        [Export] public HSlider overallVolumeSettings;
        [Export] private HSlider SoundsVolumeSettings;
        [Export] private HSlider MusicsVolumeSettings;

        public override void _Ready()
        {
            overallVolumeSettings.ValueChanged += UpdateOverallVolume;
            SoundsVolumeSettings.ValueChanged += UpdateSoundsVolume;
            MusicsVolumeSettings.ValueChanged += UpdateMusicsVolume;

            overallVolumeSettings.Value = SoundManager.GetInstance().overallVolume;
            SoundsVolumeSettings.Value = SoundManager.GetInstance().soundsVolume;
            MusicsVolumeSettings.Value = SoundManager.GetInstance().musicVolume;
        }
        
        private void UpdateOverallVolume(double value)
        {
            if (overallVolumeSettings.Value <= overallVolumeSettings.MinValue) AudioServer.SetBusMute(0, true);
            else
            {
                AudioServer.SetBusVolumeDb(0, (float)value);
                AudioServer.SetBusMute(0, false);
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
        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

            
		}

		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
