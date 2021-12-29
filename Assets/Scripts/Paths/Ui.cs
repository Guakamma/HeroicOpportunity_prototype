using HeroicOpportunity.Ui;


namespace HeroicOpportunity.Paths
{
    public static class Ui
    {
        private const string Root = Common.PrefabsRoot + "Ui/";
        private const string ScreensRoot = Root + "Screens/";
        private const string PrefixScreen = "Ui_Screens_";

        public const string AbilitiesCard = ScreensRoot + "InGame/" + "Ui_Screens_InGame_AbilityCard";
        public const string AbilitiesCardInGame = AbilitiesCard + "_inGame";

        public static string GetScreenPath(ScreenType screenType) => ScreensRoot + PrefixScreen + screenType;
    }
}
