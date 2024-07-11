using Fingers.Gameplay.Scores;
using Fingers.Gameplay.Wallet;

namespace Fingers.Core.Progress
{
    public class ProgressData
    {
        public float TimeGame;
        public int LocaleId;

        public WalletData Wallet;
        public ScoresData ScoresData;
        
        public static bool operator==(ProgressData value1, ProgressData value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return ReferenceEquals(value1, value2);
            
            return value1.TimeGame == value2.TimeGame;
        }

        public static bool operator!=(ProgressData value1, ProgressData value2)
        {
            return !(value1 == value2);
        }

        public static bool operator<(ProgressData value1, ProgressData value2)
        {
            return value1.TimeGame < value2.TimeGame;
        }

        public static bool operator>(ProgressData value1, ProgressData value2)
        {
            return !(value1 < value2);
        }

        public static bool operator<=(ProgressData value1, ProgressData value2)
        {
            return value1.TimeGame <= value2.TimeGame;
        }

        public static bool operator>=(ProgressData value1, ProgressData value2)
        {
            return !(value1 <= value2);
        }
    }
}