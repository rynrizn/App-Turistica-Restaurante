using MudBlazor;

namespace RestauranteTuristicoApp.Components
{
    public static class SaborTheme
    {
        public static MudTheme DefaultTheme => new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#610000",
                PrimaryContrastText = "#FFFFFF",
                Secondary = "#735C00",
                SecondaryContrastText = "#FFFFFF",
                Tertiary = "#2C2C2C",
                TertiaryContrastText = "#FFFFFF",
                Background = "#F8F9FA",
                Surface = "#FFFFFF",
                AppbarBackground = "#610000",
                AppbarText = "#FFFFFF",
                DrawerBackground = "#F8F9FA",
                DrawerText = "#191C1D",
                DrawerIcon = "#610000",
                TextPrimary = "#191C1D",
                TextSecondary = "#5A403C",
                ActionDefault = "#610000",
                ActionDisabled = "#B0AEAE",
                ActionDisabledBackground = "#E1E3E4",
                Divider = "#E0E0E0",
                TableLines = "#E0E0E0",
                TableHover = "rgba(97, 0, 0, 0.04)"
            },
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = "8px",
                DrawerWidthLeft = "260px"
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = new[] { "Roboto Flex", "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = "400",
                    LineHeight = "1.43",
                    LetterSpacing = ".01071em"
                },
                H1 = new H1Typography
                {
                    FontFamily = new[] { "Playfair Display", "serif" },
                    FontSize = "3.5rem",
                    FontWeight = "700",
                    LineHeight = "1.167"
                },
                H2 = new H2Typography
                {
                    FontFamily = new[] { "Playfair Display", "serif" },
                    FontSize = "2.5rem",
                    FontWeight = "700",
                    LineHeight = "1.2"
                },
                H3 = new H3Typography
                {
                    FontFamily = new[] { "Playfair Display", "serif" },
                    FontSize = "2rem",
                    FontWeight = "700",
                    LineHeight = "1.25"
                },
                H4 = new H4Typography
                {
                    FontFamily = new[] { "Playfair Display", "serif" },
                    FontSize = "1.5rem",
                    FontWeight = "600",
                    LineHeight = "1.3"
                },
                H5 = new H5Typography
                {
                    FontFamily = new[] { "Playfair Display", "serif" },
                    FontSize = "1.25rem",
                    FontWeight = "600",
                    LineHeight = "1.334"
                },
                H6 = new H6Typography
                {
                    FontFamily = new[] { "Playfair Display", "serif" },
                    FontSize = "1.125rem",
                    FontWeight = "600",
                    LineHeight = "1.6"
                },
                Button = new ButtonTypography
                {
                    FontFamily = new[] { "Roboto Flex", "Roboto", "sans-serif" },
                    FontSize = "0.875rem",
                    FontWeight = "600",
                    LetterSpacing = ".02857em"
                }
            }
        };
    }
}
