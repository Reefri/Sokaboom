using Godot;

// Author : Sacha Gratikoff

namespace Com.IsartDigital.Sokoban
{
    public partial class SoundManager : Node2D
    {
        private static PackedScene factory = (PackedScene)GD.Load("res://Scenes/Manager/SoundManager.tscn");

        private static SoundManager instance;

        private const string SOUNDS_NODE_PATH = "Sounds";
        private const string MUSICS_NODE_PATH = "Musics";

        private Node soundsContainer;
        private Node musicsContainer;

        private float musicDB = 40;
        public float MusicDB
        {
            get { return musicDB; }
            set
            {
                UpdateDB(musicsContainer, value);
                musicDB = value;
            }
        }
        private float soundDB = 40;
        public float SoundDB
        {
            get { return soundDB; }
            set
            {
                UpdateDB(soundsContainer, value);
                musicDB = value;
            }
        }

        [Export] bool debug;

        [ExportCategory("Sounds")]

        [ExportGroup("GamePlay")]

        [ExportSubgroup("bomb")]
        [Export] private AudioStreamPlayer fireworkWhistle;
        [Export] private AudioStreamPlayer bombPickUp;
        [Export] private AudioStreamPlayer fireworkExplosion;
        [Export] private AudioStreamPlayer explosion;
        [Export] private AudioStreamPlayer wallExplosion;

        [ExportSubgroup("box")]
        [Export] private AudioStreamPlayer boxError;
        [Export] private AudioStreamPlayer boxValid;
        [Export] private AudioStreamPlayer boxExplosion;
        [Export] private AudioStreamPlayer boxMove;

        [ExportSubgroup("move")]
        [Export] private AudioStreamPlayer startPathFind;
        [Export] private AudioStreamPlayer footStep;

        [ExportGroup("UI")]
        [Export] private AudioStreamPlayer click;


        private SoundManager()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(SoundManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        public static SoundManager GetInstance()
        {
            if (instance == null) instance = (SoundManager)factory.Instantiate();
            return instance;
        }

        public override void _Ready()
        {
            soundsContainer = GetNode(SOUNDS_NODE_PATH);
            musicsContainer = GetNode(MUSICS_NODE_PATH);

            UpdateDB(soundsContainer,soundDB);
            UpdateDB(musicsContainer,musicDB);
        }

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
        }


        private void UpdateDB(Node pNode, float pNewValue)
        {
            if (pNode is AudioStreamPlayer)
            {
                ((AudioStreamPlayer)pNode).VolumeDb = pNewValue;
            }

            foreach (Node lChild in pNode.GetChildren())
            {
                UpdateDB(lChild, pNewValue);
            }
        }


        public void PlayFireworkExplosion()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);


            fireworkExplosion.Play();
        }
        public void PlayFireworkWhistle()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            fireworkWhistle.Play();
        }
        public void PlayBombPickUp()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            bombPickUp.Play();
        }
        public void PlayExplosion()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            explosion.Play();
        }
        public void PlayWallExplosion()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            wallExplosion.Play();
        }
        public void PlayBoxError()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            boxError.Play();
        }
        public void PlayBoxValid()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            boxValid.Play();
        }
        public void PlayBoxExplosion()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            boxExplosion.Play();
        }
        public void PlayBoxMove()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            boxMove.Play();
        }
        public void PlayStartPathFindind()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);
            
            startPathFind.Play();
        }
        public void PlayFootStep()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            footStep.Play();
        }
        public void PlayClick()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            click.Play();
        }
    }
}
