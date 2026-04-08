using System.Collections.Generic;
using System.Linq;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class Account
	{
		public string Id 
		{ get; set; }
		public int Password { get; set; }
		public List<int> Score { get; set; }
		public List<int> BestPar { get; set; }
		public List<bool> LockedLevels { get; set; }

		public const string SCORE_STRING = "score";
        public const string PAR_STRING = "par";
        public const string LOCKEDLEVELS_STRING = "lockedLevels";


		public override string ToString()
		{
			string lRes = "Informations sur le joueur : \n";
			lRes += "Pseudo : " + Id + "\n";
			lRes += "Mdp : " + Password + "\n";

			string lTempRes = "";

			int lNumberOfScore = Score.Count;
			for (int i = 0; i < lNumberOfScore; i++)
			{
				lTempRes += Score[i] + ", ";
			}

			lRes += "Scores : " + lTempRes + "\n";

			lTempRes = "";
			int lNumberOfBestPar = BestPar.Count;
			for (int i = 0; i < lNumberOfScore; i++)
			{
				lTempRes += BestPar[i] + ", ";
			}

			lRes += "Par : " + lTempRes + "\n";


			lTempRes = "";

			int lNumberOfLockedLevels = LockedLevels.Count;


            for (int i = 0; i < lNumberOfLockedLevels; i++)
			{
				lTempRes += LockedLevels[i] + ", ";
			}

			lRes += "Niveaux vérouiller : " + lTempRes + "\n";


			return lRes;
		}

		public static Account Create(string pId,int pPassword)
		{
			Account lNewAccount = new Account();
			lNewAccount.Id = pId;
			lNewAccount.Password = pPassword;
			lNewAccount.Score = new List<int>(new int[GridManager.GetInstance().numberOfLevel]);
            lNewAccount.BestPar = new List<int>(new int[GridManager.GetInstance().numberOfLevel]);
            lNewAccount.LockedLevels = new List<bool>(new bool[GridManager.GetInstance().numberOfLevel]);
			lNewAccount.LockedLevels[0] = true;

			return lNewAccount;
        }

		public void Update()
		{
            if (LockedLevels.Count != GridManager.GetInstance().numberOfLevel)
			{
                Score = new List<int>(new int[GridManager.GetInstance().numberOfLevel]);
                BestPar = new List<int>(new int[GridManager.GetInstance().numberOfLevel]);
                LockedLevels = new List<bool>(new bool[GridManager.GetInstance().numberOfLevel]);
                LockedLevels[0] = true;
                AccountManager.GetInstance().UpdateAccount();
            }
        }

        public float FinalScore()
		{
			return Score.Sum();
		}
    }
}
