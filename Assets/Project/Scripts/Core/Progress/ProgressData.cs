using EmpireCafe.Gameplay.Wallet;

namespace EmpireCafe.Core.Progress
{
    public class ProgressData
    {
        public float TimeGame;

        public WalletData Wallet;
        
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