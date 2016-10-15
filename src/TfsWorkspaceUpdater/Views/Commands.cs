namespace TfsWorkspaceUpdater.Views
{
    using System.Windows.Input;

    public static class Commands
    {
        public static readonly RoutedUICommand OpenConfigurationCommand = new RoutedUICommand(
            "Open configuration",
            "OpenConfiguration",
            typeof(Commands));

        public static readonly RoutedUICommand SaveConfigurationCommand = new RoutedUICommand(
            "Save configuration",
            "SaveConfiguration",
            typeof(Commands));
    }
}