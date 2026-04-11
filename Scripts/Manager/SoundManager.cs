using Godot;
using System.Collections.Generic;
using System.Linq;

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

        [Export] private float musicDB = 40;
        public float MusicDB
        {
            get { return musicDB; }
            set
            {
                UpdateDB(musicsContainer, value);
                musicDB = value;
            }
        }

        [Export] private float soundDB = 40;
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
        [Export] private AudioStreamPlayer collide;
        [Export] private AudioStreamPlayer footStepOne;
        [Export] private AudioStreamPlayer footStepTwo;

        List<AudioStreamPlayer> footStepSounds;

        [ExportGroup("UI")]
        [Export] private AudioStreamPlayer sparkles;
        [Export] private AudioStreamPlayer starOne;
        [Export] private AudioStreamPlayer starTwo;
        [Export] private AudioStreamPlayer starThree;
        [Export] private AudioStreamPlayer door;
        [Export] private AudioStreamPlayer cloud;
        [Export] private AudioStreamPlayer ruban;
        [Export] private AudioStreamPlayer click;

        List<System.Action> playStarSound;


        [ExportCategory("Musics")]
        [Export] private AudioStreamPlayer music;


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

            playStarSound = new List<System.Action> 
            {
                PlayStarOne, PlayStarTwo, PlayStarThree,
            };


            footStepSounds = new List<AudioStreamPlayer>
            {
                //footStepOne,
                footStepTwo
            };

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

            List<Node> lChildren = pNode.GetChildren().ToList();

            foreach (Node lChild in lChildren)
            {
                UpdateDB(lChild, pNewValue);
            }
        }

        //########################################################################################################################
        //############################################          EPXLOSION         ################################################
        //########################################################################################################################
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


        //########################################################################################################################
        //############################################             BOX            ################################################
        //########################################################################################################################

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



        //########################################################################################################################
        //############################################            MOVE            ################################################
        //########################################################################################################################

        public void PlayStartPathFindind()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);
            
            startPathFind.Play();
        }
        public void PlayFootStep()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            footStepSounds[GD.RandRange(0,footStepSounds.Count-1)].Play();
        }

        public void PlayCollide()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            collide.Play();
        }



        //########################################################################################################################
        //############################################              UI            ################################################
        //########################################################################################################################

        public void PlaySparkles() //???
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            sparkles.Play();
        }
        public void PlayStarOne()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            starOne.Play();
        }
        public void PlayStarTwo()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            starTwo.Play();
        }
        public void PlayStarThree()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            starThree.Play();
        }


        public void PlayStarIndex(int pIndex)
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            playStarSound[Mathf.Clamp(pIndex, 0, playStarSound.Count-1)].Invoke();

        }
        public void PlayDoor()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            door.Play();
        }
        public void PlayCloud()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            cloud.Play();
        }
        public void PlayRuban()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            ruban.Play();
        }
        public void PlayClick()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            click.Play();
        }




        //########################################################################################################################
        //############################################            MUSIC           ################################################
        //########################################################################################################################


        public void PlayMusic()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            music.Play();
        }

        public void StopMusic()
        {
            if (debug) GD.Print(System.Reflection.MethodBase.GetCurrentMethod().Name);

            music.Stop();
        }
    }
}
