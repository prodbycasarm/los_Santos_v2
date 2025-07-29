using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using static losSantosV2.VehicleColors;

public class losSantosV2 : Script
{


    private bool isNearUpgradeZone = false;
    private List<Vector3> upgradeZonePositions = new List<Vector3>();
    private float upgradeZoneRadius = 6.0f;
    private List<Blip> upgradeZoneBlips = new List<Blip>();

    private ObjectPool pool;
    private NativeMenu opendoorsSubMenu;
    private NativeMenu turboSubMenu;
    private NativeMenu upgradeMenu;
    private NativeMenu paintSubMenu;
    private NativeMenu addNeonSubMenu;
    private NativeMenu tintColorSubMenu;
    private NativeMenu headlightColorSubMenu;
    private NativeMenu neonColorSubMenu;
    private NativeMenu engineSubMenu;
    private NativeMenu armorSubMenu;
    private NativeMenu headlightSubMenu;
    private NativeMenu brakeSubMenu;
    private NativeMenu transmissionSubMenu;
    private NativeMenu suspensionSubMenu;
    private int _originalWheelType = -1;
    private int _originalRimIndex = -1;
    private bool _hasCachedOriginal = false;
    ScriptSettings config;
    Keys enable;
    public static class VehicleColors
    {
        public class VehicleColorInfo
        {
            public int Color { get; set; }
            public int Spec { get; set; }
        }
        // Classic Colors
        public static readonly Dictionary<string, VehicleColorInfo> ClassicColors = new Dictionary<string, VehicleColorInfo>()
        {
            { "Black", new VehicleColorInfo { Color = 0, Spec = 0 } },
            { "Black Graphite", new VehicleColorInfo { Color = 147, Spec = 0 } },
            { "Graphite", new VehicleColorInfo { Color = 1, Spec = 0 } },
            { "Anthracite Black", new VehicleColorInfo { Color = 11, Spec = 0 } },
            { "Black Steel", new VehicleColorInfo { Color = 2, Spec = 0 } },
            { "Dark Silver", new VehicleColorInfo { Color = 3, Spec = 2 } },
            { "Silver", new VehicleColorInfo { Color = 4, Spec = 4 } },
            { "Blue Silver", new VehicleColorInfo { Color = 5, Spec = 5 } },
            { "Rolled Steel", new VehicleColorInfo { Color = 6, Spec = 0 } },
            { "Shadow Silver", new VehicleColorInfo { Color = 7, Spec = 0 } },
            { "Stone Silver", new VehicleColorInfo { Color = 8, Spec = 0 } },
            { "Midnight Silver", new VehicleColorInfo { Color = 9, Spec = 0 } },
            { "Cast Iron Silver", new VehicleColorInfo { Color = 10, Spec = 0 } },
            { "Red", new VehicleColorInfo { Color = 27, Spec = 0 } },
            { "Torino Red", new VehicleColorInfo { Color = 28, Spec = 0 } },
            { "Formula Red", new VehicleColorInfo { Color = 29, Spec = 0 } },
            { "Lava Red", new VehicleColorInfo { Color = 150, Spec = 0 } },
            { "Blaze Red", new VehicleColorInfo { Color = 30, Spec = 0 } },
            { "Grace Red", new VehicleColorInfo { Color = 31, Spec = 0 } },
            { "Garnet Red", new VehicleColorInfo { Color = 32, Spec = 0 } },
            { "Sunset Red", new VehicleColorInfo { Color = 33, Spec = 0 } },
            { "Cabernet Red", new VehicleColorInfo { Color = 34, Spec = 0 } },
            { "Wine Red", new VehicleColorInfo { Color = 143, Spec = 0 } },
            { "Candy Red", new VehicleColorInfo { Color = 35, Spec = 0 } },
            { "Hot Pink", new VehicleColorInfo { Color = 135, Spec = 0 } },
            { "Pink", new VehicleColorInfo { Color = 137, Spec = 0 } },
            { "Salmon Pink", new VehicleColorInfo { Color = 136, Spec = 0 } },
            { "Sunrise Orange", new VehicleColorInfo { Color = 36, Spec = 0 } },
            { "Orange", new VehicleColorInfo { Color = 38, Spec = 0 } },
            { "Bright Orange", new VehicleColorInfo { Color = 138, Spec = 0 } },
            { "Gold", new VehicleColorInfo { Color = 99, Spec = 99 } },
            { "Bronze", new VehicleColorInfo { Color = 90, Spec = 102 } },
            { "Yellow", new VehicleColorInfo { Color = 88, Spec = 0 } },
            { "Race Yellow", new VehicleColorInfo { Color = 89, Spec = 0 } },
            { "Fluorescent Yellow", new VehicleColorInfo { Color = 91, Spec = 0 } },
            { "Dark Green", new VehicleColorInfo { Color = 49, Spec = 0 } },
            { "Racing Green", new VehicleColorInfo { Color = 50, Spec = 0 } },
            { "Sea Green", new VehicleColorInfo { Color = 51, Spec = 0 } },
            { "Olive Green", new VehicleColorInfo { Color = 52, Spec = 0 } },
            { "Bright Green", new VehicleColorInfo { Color = 53, Spec = 0 } },
            { "Petrol Green", new VehicleColorInfo { Color = 54, Spec = 0 } },
            { "Lime Green", new VehicleColorInfo { Color = 92, Spec = 0 } },
            { "Midnight Blue", new VehicleColorInfo { Color = 141, Spec = 0 } },
            { "Galaxy Blue", new VehicleColorInfo { Color = 61, Spec = 0 } },
            { "Dark Blue", new VehicleColorInfo { Color = 62, Spec = 0 } },
            { "Saxon Blue", new VehicleColorInfo { Color = 63, Spec = 0 } },
            { "Blue", new VehicleColorInfo { Color = 64, Spec = 0 } },
            { "Mariner Blue", new VehicleColorInfo { Color = 65, Spec = 0 } },
            { "Harbor Blue", new VehicleColorInfo { Color = 66, Spec = 0 } },
            { "Diamond Blue", new VehicleColorInfo { Color = 67, Spec = 0 } },
            { "Surf Blue", new VehicleColorInfo { Color = 68, Spec = 0 } },
            { "Nautical Blue", new VehicleColorInfo { Color = 69, Spec = 0 } },
            { "Racing Blue", new VehicleColorInfo { Color = 73, Spec = 0 } },
            { "Ultra Blue", new VehicleColorInfo { Color = 70, Spec = 0 } },
            { "Light Blue", new VehicleColorInfo { Color = 74, Spec = 0 } },
            { "Chocolate Brown", new VehicleColorInfo { Color = 96, Spec = 0 } },
            { "Bison Brown", new VehicleColorInfo { Color = 101, Spec = 0 } },
            { "Creek Brown", new VehicleColorInfo { Color = 95, Spec = 0 } },
            { "Umber Brown", new VehicleColorInfo { Color = 94, Spec = 0 } },
            { "Maple Brown", new VehicleColorInfo { Color = 97, Spec = 0 } },
            { "Beechwood Brown", new VehicleColorInfo { Color = 103, Spec = 0 } },
            { "Sienna Brown", new VehicleColorInfo { Color = 104, Spec = 0 } },
            { "Saddle Brown", new VehicleColorInfo { Color = 98, Spec = 0 } },
            { "Moss Brown", new VehicleColorInfo { Color = 100, Spec = 0 } },
            { "Woodbeech Brown", new VehicleColorInfo { Color = 102, Spec = 0 } },
            { "Straw Brown", new VehicleColorInfo { Color = 99, Spec = 0 } },
            { "Sandy Brown", new VehicleColorInfo { Color = 105, Spec = 0 } },
            { "Bleached Brown", new VehicleColorInfo { Color = 106, Spec = 0 } },
            { "Purple", new VehicleColorInfo { Color = 71, Spec = 0 } },
            { "Spin Purple", new VehicleColorInfo { Color = 72, Spec = 0 } },
            { "Might Purple", new VehicleColorInfo { Color = 142, Spec = 0 } },
            { "Bright Purple", new VehicleColorInfo { Color = 145, Spec = 0 } },
            { "Cream", new VehicleColorInfo { Color = 107, Spec = 0 } },
            { "White", new VehicleColorInfo { Color = 111, Spec = 0 } },
            { "Frost White", new VehicleColorInfo { Color = 112, Spec = 0 } }
        };
        // Metallic Colors
        public static readonly Dictionary<string, VehicleColorInfo> MetallicColors = new Dictionary<string, VehicleColorInfo>()
{
    { "Black", new VehicleColorInfo { Color = 0, Spec = 10 } },
    { "BlackGraphite", new VehicleColorInfo { Color = 147, Spec = 4 } },
    { "Graphite", new VehicleColorInfo { Color = 1, Spec = 5 } },
    { "AnthrBlack", new VehicleColorInfo { Color = 11, Spec = 2 } },
    { "BlackSteel", new VehicleColorInfo { Color = 2, Spec = 5 } },
    { "DarkSilver", new VehicleColorInfo { Color = 3, Spec = 6 } },
    { "Silver", new VehicleColorInfo { Color = 4, Spec = 111 } },
    { "BlueSilver", new VehicleColorInfo { Color = 5, Spec = 111 } },
    { "RolledSteel", new VehicleColorInfo { Color = 6, Spec = 4 } },
    { "ShadowSilver", new VehicleColorInfo { Color = 7, Spec = 5 } },
    { "StoneSilver", new VehicleColorInfo { Color = 8, Spec = 5 } },
    { "MidnightSilver", new VehicleColorInfo { Color = 9, Spec = 7 } },
    { "CastIronSil", new VehicleColorInfo { Color = 10, Spec = 7 } },
    { "Red", new VehicleColorInfo { Color = 27, Spec = 36 } },
    { "TorinoRed", new VehicleColorInfo { Color = 28, Spec = 28 } },
    { "FormulaRed", new VehicleColorInfo { Color = 29, Spec = 28 } },
    { "LavaRed", new VehicleColorInfo { Color = 150, Spec = 42 } },
    { "BlazeRed", new VehicleColorInfo { Color = 30, Spec = 36 } },
    { "GraceRed", new VehicleColorInfo { Color = 31, Spec = 27 } },
    { "GarnetRed", new VehicleColorInfo { Color = 32, Spec = 25 } },
    { "SunsetRed", new VehicleColorInfo { Color = 33, Spec = 47 } },
    { "CabernetRed", new VehicleColorInfo { Color = 34, Spec = 47 } },
    { "WineRed", new VehicleColorInfo { Color = 143, Spec = 31 } },
    { "CandyRed", new VehicleColorInfo { Color = 35, Spec = 25 } },
    { "HotPink", new VehicleColorInfo { Color = 135, Spec = 135 } },
    { "Pink", new VehicleColorInfo { Color = 137, Spec = 3 } },
    { "SalmonPink", new VehicleColorInfo { Color = 136, Spec = 5 } },
    { "SunriseOrange", new VehicleColorInfo { Color = 36, Spec = 26 } },
    { "Orange", new VehicleColorInfo { Color = 38, Spec = 37 } },
    { "BrightOrange", new VehicleColorInfo { Color = 138, Spec = 89 } },
    { "Gold", new VehicleColorInfo { Color = 37, Spec = 106 } },
    { "Bronze", new VehicleColorInfo { Color = 90, Spec = 102 } },
    { "Yellow", new VehicleColorInfo { Color = 88, Spec = 88 } },
    { "RaceYellow", new VehicleColorInfo { Color = 89, Spec = 88 } },
    { "FlurYellow", new VehicleColorInfo { Color = 91, Spec = 91 } },
    { "DarkGreen", new VehicleColorInfo { Color = 49, Spec = 52 } },
    { "RacingGreen", new VehicleColorInfo { Color = 50, Spec = 53 } },
    { "SeaGreen", new VehicleColorInfo { Color = 51, Spec = 66 } },
    { "OliveGreen", new VehicleColorInfo { Color = 52, Spec = 59 } },
    { "BrightGreen", new VehicleColorInfo { Color = 53, Spec = 59 } },
    { "PetrolGreen", new VehicleColorInfo { Color = 54, Spec = 60 } },
    { "LimeGreen", new VehicleColorInfo { Color = 92, Spec = 92 } },
    { "MidnightBlue", new VehicleColorInfo { Color = 141, Spec = 73 } },
    { "GalaxyBlue", new VehicleColorInfo { Color = 61, Spec = 63 } },
    { "DarkBlue", new VehicleColorInfo { Color = 62, Spec = 68 } },
    { "SaxonBlue", new VehicleColorInfo { Color = 63, Spec = 87 } },
    { "Blue", new VehicleColorInfo { Color = 64, Spec = 68 } },
    { "MarinerBlue", new VehicleColorInfo { Color = 65, Spec = 87 } },
    { "HarborBlue", new VehicleColorInfo { Color = 66, Spec = 60 } },
    { "DiamondBlue", new VehicleColorInfo { Color = 67, Spec = 67 } },
    { "SurfBlue", new VehicleColorInfo { Color = 68, Spec = 68 } },
    { "NauticalBlue", new VehicleColorInfo { Color = 69, Spec = 74 } },
    { "RacingBlue", new VehicleColorInfo { Color = 73, Spec = 73 } },
    { "UltraBlue", new VehicleColorInfo { Color = 70, Spec = 70 } },
    { "LightBlue", new VehicleColorInfo { Color = 74, Spec = 74 } },
    { "ChocolateBrown", new VehicleColorInfo { Color = 96, Spec = 95 } },
    { "BisonBrown", new VehicleColorInfo { Color = 101, Spec = 95 } },
    { "CreekBrown", new VehicleColorInfo { Color = 95, Spec = 97 } },
    { "UmberBrown", new VehicleColorInfo { Color = 94, Spec = 104 } },
    { "MapleBrown", new VehicleColorInfo { Color = 97, Spec = 98 } },
    { "BeechwoodBrown", new VehicleColorInfo { Color = 103, Spec = 104 } },
    { "SiennaBrown", new VehicleColorInfo { Color = 104, Spec = 104 } },
    { "SaddleBrown", new VehicleColorInfo { Color = 98, Spec = 95 } },
    { "MossBrown", new VehicleColorInfo { Color = 100, Spec = 100 } },
    { "WoodbeechBrown", new VehicleColorInfo { Color = 102, Spec = 105 } },
    { "StrawBrown", new VehicleColorInfo { Color = 99, Spec = 106 } },
    { "SandyBrown", new VehicleColorInfo { Color = 105, Spec = 105 } },
    { "BleachedBrown", new VehicleColorInfo { Color = 106, Spec = 106 } },
    { "Purple", new VehicleColorInfo { Color = 71, Spec = 145 } },
    { "SpinPurple", new VehicleColorInfo { Color = 72, Spec = 64 } },
    { "MightPurple", new VehicleColorInfo { Color = 146, Spec = 145 } },
    { "BrightPurple", new VehicleColorInfo { Color = 145, Spec = 74 } },
    { "Cream", new VehicleColorInfo { Color = 107, Spec = 107 } },
    { "White", new VehicleColorInfo { Color = 111, Spec = 0 } },
    { "FrostWhite", new VehicleColorInfo { Color = 112, Spec = 0 } }
};



        // Matte Colors
        public static readonly Dictionary<string, VehicleColorInfo> MatteColors = new Dictionary<string, VehicleColorInfo>()
        {
            { "Matte Black", new VehicleColorInfo { Color = 12, Spec = 0 } },
            { "Matte Gray", new VehicleColorInfo { Color = 13, Spec = 0 } },
            { "Matte Light Gray", new VehicleColorInfo { Color = 14, Spec = 0 } },
            { "Matte Red", new VehicleColorInfo { Color = 39, Spec = 0 } },
            { "Matte Dark Red", new VehicleColorInfo { Color = 40, Spec = 0 } },
            { "Matte Orange", new VehicleColorInfo { Color = 41, Spec = 0 } },
            { "Matte Yellow", new VehicleColorInfo { Color = 42, Spec = 0 } },
            { "Matte Lime Green", new VehicleColorInfo { Color = 55, Spec = 0 } },
            { "Matte Blue", new VehicleColorInfo { Color = 82, Spec = 0 } },
            { "Matte Dark Blue", new VehicleColorInfo { Color = 83, Spec = 0 } },
            { "Matte Midnight Blue", new VehicleColorInfo { Color = 84, Spec = 0 } },
            { "Matte Green", new VehicleColorInfo { Color = 55, Spec = 0 } },
            { "Matte Brown", new VehicleColorInfo { Color = 94, Spec = 0 } },
            { "Matte White", new VehicleColorInfo { Color = 131, Spec = 0 } },
            { "Matte Might Purple", new VehicleColorInfo { Color = 149, Spec = 0 } },
            { "Matte Purple", new VehicleColorInfo { Color = 148, Spec = 0 } },
            { "Matte FOR", new VehicleColorInfo { Color = 151, Spec = 0 } },
            { "Matte Foil", new VehicleColorInfo { Color = 155, Spec = 0 } },
            { "Matte OD", new VehicleColorInfo { Color = 152, Spec = 0 } },
            { "Matte Dirt", new VehicleColorInfo { Color = 153, Spec = 0 } },
            { "Matte Desert", new VehicleColorInfo { Color = 154, Spec = 0 } }
        };

        // Metals Colors
        public static readonly Dictionary<string, VehicleColorInfo> Metals = new Dictionary<string, VehicleColorInfo>()
        {
            { "Brushed Steel", new VehicleColorInfo { Color = 117, Spec = 18 } },
            { "Brushed Black Steel", new VehicleColorInfo { Color = 118, Spec = 3 } },
            { "Brushed Aluminium", new VehicleColorInfo { Color = 119, Spec = 5 } },
            { "Pure Gold", new VehicleColorInfo { Color = 158, Spec = 160 } },
            { "Brushed Gold", new VehicleColorInfo { Color = 159, Spec = 160 } }
        };

        // Chrome Colors
        public static readonly Dictionary<string, VehicleColorInfo> ChromeColors = new Dictionary<string, VehicleColorInfo>()
{
    { "Chrome", new VehicleColorInfo { Color = 120, Spec = 0 } },
    // --- G9_PAINTxx Entries (Second List) ---
    { "G9_PAINT01", new VehicleColorInfo { Color = 161, Spec = 0 } },
    { "G9_PAINT02", new VehicleColorInfo { Color = 162, Spec = 0 } },
    { "G9_PAINT03", new VehicleColorInfo { Color = 163, Spec = 0 } },
    { "G9_PAINT04", new VehicleColorInfo { Color = 164, Spec = 0 } },
    { "G9_PAINT05", new VehicleColorInfo { Color = 165, Spec = 0 } },
    { "G9_PAINT06", new VehicleColorInfo { Color = 166, Spec = 0 } },
    { "G9_PAINT07", new VehicleColorInfo { Color = 167, Spec = 0 } },
    { "G9_PAINT08", new VehicleColorInfo { Color = 168, Spec = 0 } },
    { "G9_PAINT09", new VehicleColorInfo { Color = 169, Spec = 0 } },
    { "G9_PAINT10", new VehicleColorInfo { Color = 170, Spec = 0 } },
    { "G9_PAINT11", new VehicleColorInfo { Color = 171, Spec = 0 } },
    { "G9_PAINT12", new VehicleColorInfo { Color = 172, Spec = 0 } },
    { "G9_PAINT13", new VehicleColorInfo { Color = 173, Spec = 0 } },
    { "G9_PAINT14", new VehicleColorInfo { Color = 174, Spec = 0 } },
    { "G9_PAINT15", new VehicleColorInfo { Color = 175, Spec = 0 } },
    { "G9_PAINT16", new VehicleColorInfo { Color = 176, Spec = 0 } },
    { "G9_PAINT17", new VehicleColorInfo { Color = 177, Spec = 0 } },
    { "G9_PAINT18", new VehicleColorInfo { Color = 179, Spec = 178 } }
};



        public static readonly Dictionary<string, VehicleColorInfo> PearlescentColors = new Dictionary<string, VehicleColorInfo>()
{
    { "Remove Pearlescent", new VehicleColorInfo { Color = 0, Spec = 0 } },
    { "Graphite", new VehicleColorInfo { Color = 1, Spec = 0 } },
    { "Black Steel", new VehicleColorInfo { Color = 2, Spec = 0 } },
    { "Dark Silver", new VehicleColorInfo { Color = 3, Spec = 0 } },
    { "Silver", new VehicleColorInfo { Color = 4, Spec = 0 } },
    { "Blue Silver", new VehicleColorInfo { Color = 5, Spec = 0 } },
    { "Rolled Steel", new VehicleColorInfo { Color = 6, Spec = 0 } },
    { "Shadow Silver", new VehicleColorInfo { Color = 7, Spec = 0 } },
    { "Stone Silver", new VehicleColorInfo { Color = 8, Spec = 0 } },
    { "Midnight Silver", new VehicleColorInfo { Color = 9, Spec = 0 } },
    { "Cast Iron Silver", new VehicleColorInfo { Color = 10, Spec = 0 } },
    { "Anthracite Black", new VehicleColorInfo { Color = 11, Spec = 0 } },
    { "Red", new VehicleColorInfo { Color = 27, Spec = 0 } },
    { "Torino Red", new VehicleColorInfo { Color = 28, Spec = 0 } },
    { "Formula Red", new VehicleColorInfo { Color = 29, Spec = 0 } },
    { "Lava Red", new VehicleColorInfo { Color = 30, Spec = 0 } },
    { "Blaze Red", new VehicleColorInfo { Color = 31, Spec = 0 } },
    { "Grace Red", new VehicleColorInfo { Color = 32, Spec = 0 } },
    { "Garnet Red", new VehicleColorInfo { Color = 33, Spec = 0 } },
    { "Sunset Red", new VehicleColorInfo { Color = 34, Spec = 0 } },
    { "Cabernet Red", new VehicleColorInfo { Color = 35, Spec = 0 } },
    { "Candy Red", new VehicleColorInfo { Color = 36, Spec = 0 } },
    { "Sunrise Orange", new VehicleColorInfo { Color = 37, Spec = 0 } },
    { "Orange", new VehicleColorInfo { Color = 38, Spec = 0 } },
    { "Bright Orange", new VehicleColorInfo { Color = 41, Spec = 0 } },
    { "Gold", new VehicleColorInfo { Color = 37, Spec = 0 } },
    { "Classic Gold", new VehicleColorInfo { Color = 37, Spec = 0 } },
    { "Yellow", new VehicleColorInfo { Color = 42, Spec = 0 } },
    { "Race Yellow", new VehicleColorInfo { Color = 88, Spec = 0 } },
    { "Dew Yellow", new VehicleColorInfo { Color = 89, Spec = 0 } },
    { "Dark Green", new VehicleColorInfo { Color = 49, Spec = 0 } },
    { "Racing Green", new VehicleColorInfo { Color = 50, Spec = 0 } },
    { "Sea Green", new VehicleColorInfo { Color = 51, Spec = 0 } },
    { "Olive Green", new VehicleColorInfo { Color = 52, Spec = 0 } },
    { "Bright Green", new VehicleColorInfo { Color = 53, Spec = 0 } },
    { "Gasoline Green", new VehicleColorInfo { Color = 54, Spec = 0 } },
    { "Lime Green", new VehicleColorInfo { Color = 55, Spec = 0 } },
    { "Midnight Blue", new VehicleColorInfo { Color = 62, Spec = 0 } },
    { "Galaxy Blue", new VehicleColorInfo { Color = 63, Spec = 0 } },
    { "Dark Blue", new VehicleColorInfo { Color = 64, Spec = 0 } },
    { "Saxon Blue", new VehicleColorInfo { Color = 65, Spec = 0 } },
    { "Blue", new VehicleColorInfo { Color = 66, Spec = 0 } },
    { "Mariner Blue", new VehicleColorInfo { Color = 67, Spec = 0 } },
    { "Harbor Blue", new VehicleColorInfo { Color = 68, Spec = 0 } },
    { "Diamond Blue", new VehicleColorInfo { Color = 69, Spec = 0 } },
    { "Surf Blue", new VehicleColorInfo { Color = 70, Spec = 0 } },
    { "Nautical Blue", new VehicleColorInfo { Color = 71, Spec = 0 } },
    { "Ultra Blue", new VehicleColorInfo { Color = 73, Spec = 0 } },
    { "Light Blue", new VehicleColorInfo { Color = 74, Spec = 0 } },
    { "Chocolate Brown", new VehicleColorInfo { Color = 96, Spec = 0 } },
    { "Bison Brown", new VehicleColorInfo { Color = 101, Spec = 0 } },
    { "Creek Brown", new VehicleColorInfo { Color = 95, Spec = 0 } },
    { "Feltzer Brown", new VehicleColorInfo { Color = 94, Spec = 0 } },
    { "Maple Brown", new VehicleColorInfo { Color = 97, Spec = 0 } },
    { "Beechwood Brown", new VehicleColorInfo { Color = 103, Spec = 0 } },
    { "Sienna Brown", new VehicleColorInfo { Color = 104, Spec = 0 } },
    { "Saddle Brown", new VehicleColorInfo { Color = 98, Spec = 0 } },
    { "Moss Brown", new VehicleColorInfo { Color = 100, Spec = 0 } },
    { "Woodbeech Brown", new VehicleColorInfo { Color = 102, Spec = 0 } },
    { "Straw Brown", new VehicleColorInfo { Color = 99, Spec = 0 } },
    { "Sandy Brown", new VehicleColorInfo { Color = 105, Spec = 0 } },
    { "Bleached Brown", new VehicleColorInfo { Color = 106, Spec = 0 } },
    { "Schafter Purple", new VehicleColorInfo { Color = 71, Spec = 0 } },
    { "Spinnaker Purple", new VehicleColorInfo { Color = 72, Spec = 0 } },
    { "Midnight Purple", new VehicleColorInfo { Color = 142, Spec = 0 } },
    { "Bright Purple", new VehicleColorInfo { Color = 145, Spec = 0 } },
    { "Cream", new VehicleColorInfo { Color = 107, Spec = 0 } },
    { "Ice White", new VehicleColorInfo { Color = 111, Spec = 0 } },
    { "Frost White", new VehicleColorInfo { Color = 112, Spec = 0 } }
};

    }

    // neon prices for each level, ordered from 1 to 4
    private Dictionary<int, int> neonPrices = new Dictionary<int, int>
    {
        { 1, 1000 },
        { 2, 1000 },
        { 3, 1000 },
        { 4, 1000 },
        { 5, 4000 }
    };

    // Armor prices for each level, ordered from 1 to 4
    private Dictionary<int, int> armorPrices = new Dictionary<int, int>
    {
        { 1, 1000 },
        { 2, 5000 },
        { 3, 8000 },
        { 4, 10000 },
        { 5, 12000 }
    };

    // Headlights prices
    private Dictionary<int, int> headlightPrices = new Dictionary<int, int>
    {
        { 1, 2000 }, // Xenon installation
        { 2, 500 }   // Reverting to normal headlights
    };

    // Turbo prices
    private Dictionary<int, int> TurboPrices = new Dictionary<int, int>
    {
        { 1, 5000 }, // Turbo installation
        { 2, 500 }   // Reverting to normal
    };

    // Brake prices for each level, ordered from 1 to 3
    private Dictionary<int, int> brakePrices = new Dictionary<int, int>
    {
        { 1, 500 },
        { 2, 1500 },
        { 3, 2500 }
    };

    // Transmission prices for each level, ordered from 1 to 3
    private Dictionary<int, int> transmissionPrices = new Dictionary<int, int>
    {
        { 1, 1500 },
        { 2, 2500 },
        { 3, 3500 }
    };

    // Suspension prices for each level, ordered from 1 to 3
    private Dictionary<int, int> suspensionPrices = new Dictionary<int, int>
    {
        { 1, 1500 },
        { 2, 2000 },
        { 3, 4500 },
        { 4, 5500 }
    };

    // Engine prices for each level, ordered from 1 to 4
    private Dictionary<int, int> enginePrices = new Dictionary<int, int>
    {
        { 1, 1500 },
        { 2, 3000 },
        { 3, 5500 },
        { 4, 8500 }
    };
    private int baseRepairCost = 500;
    private NativeItem installTurboItem;
    private NativeItem removeTurboItem;


    public losSantosV2()
    {
        Tick += OnTick;
        KeyUp += OnKeyUp;

        config = ScriptSettings.Load("scripts\\config_ls2.ini");
        string enableKeyString = config.GetValue<string>("Options", "Button", "Enter");
        if (!Enum.TryParse(enableKeyString, out enable))
        {
            enable = Keys.Enter;
            Notification.Show("Failed to parse key, using default 'Enter'");
        }
        LoadUpgradeZonePositions();


        // Load the blip settings from the config file
        string blipSpriteString = config.GetValue<string>("Blip", "Sprite", "LosSantosCustoms");
        string blipColorString = config.GetValue<string>("Blip", "Color", "BlueLight");
        string blipName = config.GetValue<string>("Blip", "Name", "Custom Workshop");

        // Debug messages to verify values
        GTA.UI.Notification.Show($"Blip Settings: Sprite={blipSpriteString}, Color={blipColorString}, Name={blipName}");

        // Create the upgrade zone blip
        CreateUpgradeZoneBlips(blipSpriteString, blipColorString, blipName);


        // Initialize LemonUI components
        pool = new ObjectPool();



        upgradeMenu = new NativeMenu(
            "",
            "CATEGORIES",
            "Categories",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod", 
                "shopui_title_carmod"
            )
        ); 
        pool.Add(upgradeMenu);

        // Repair Car
        var repairItem = new NativeItem("Repair Vehicle");
        repairItem.Activated += (sender, args) => RepairCar();
        upgradeMenu.Add(repairItem);

        //opendoors
        // Define the sub-menu for doors
        opendoorsSubMenu = new NativeMenu(
        "", // Title (empty or optional)
        "Doors", // Subtitle
        "", // ID or name
        new ScaledTexture(
            PointF.Empty,
            new SizeF(431, 107),
            "shopui_title_carmod",
            "shopui_title_carmod"
        )
        );

        pool.Add(opendoorsSubMenu);
        var opendoorsSubMenuItem = upgradeMenu.AddSubMenu(opendoorsSubMenu);

        // doors mod levels, now ordered from Level 1 to Level 7
        // Door definitions
        var doorLevels = new Dictionary<int, string>
        {
            { 1, "Bonnet" },
            { 2, "Trunk" },
            { 3, "Left Front Door" },
            { 4, "Right Front Door" },
            { 5, "Left Back Door" },
            { 6, "Right Back Door" },
            { 7, "All" }
        };

        var doorItems = new List<NativeItem>();
        var doorStates = new Dictionary<int, bool>();

        foreach (var door in doorLevels)
        {
            var doorItem = new NativeItem($"{door.Value} - Closed");
            doorStates[door.Key] = false;

            doorItem.Activated += (sender, args) =>
            {
                Vehicle vehicle = Game.Player.Character.CurrentVehicle;
                if (vehicle == null || !vehicle.Exists())
                {
                    GTA.UI.Notification.Show("~r~You must be in a vehicle!");
                    return;
                }

                bool isOpen = doorStates[door.Key];
                ToggleDoor(door.Key, !isOpen);
                doorStates[door.Key] = !isOpen;

                doorItem.Title = $"{door.Value} - {(isOpen ? "Closed" : "Opened")}";

                // Reset all other doors to closed visually
                foreach (var item in doorItems)
                {
                    if (item != doorItem)
                    {
                        var otherKey = doorLevels.First(x => item.Title.StartsWith(x.Value)).Key;
                        doorStates[otherKey] = false;
                        item.Title = $"{doorLevels[otherKey]} - Closed";
                    }
                }
            };

            doorItems.Add(doorItem);
            opendoorsSubMenu.Add(doorItem);
        }

        // Update door status when menu is opened
        opendoorsSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;

            if (vehicle != null && vehicle.Exists())
            {
                foreach (var door in doorLevels)
                {
                    int key = door.Key;
                    int doorIndex = -1;

                    switch (key)
                    {
                        case 1: doorIndex = 4; break; // Bonnet
                        case 2: doorIndex = 5; break; // Trunk
                        case 3: doorIndex = 0; break; // Left Front
                        case 4: doorIndex = 1; break; // Right Front
                        case 5: doorIndex = 2; break; // Left Rear
                        case 6: doorIndex = 3; break; // Right Rear
                    }

                    var item = doorItems[key - 1];

                    if (key == 7) // All
                    {
                        bool anyOpen = false;
                        for (int i = 0; i <= 5; i++)
                        {
                            if (Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, vehicle, i) > 0.01f)
                            {
                                anyOpen = true;
                                break;
                            }
                        }
                        item.Title = $"All - {(anyOpen ? "Opened" : "Closed")}";
                        doorStates[key] = anyOpen;
                    }
                    else if (doorIndex >= 0)
                    {
                        bool isOpen = Function.Call<float>(Hash.GET_VEHICLE_DOOR_ANGLE_RATIO, vehicle, doorIndex) > 0.01f;
                        item.Title = $"{door.Value} - {(isOpen ? "Opened" : "Closed")}";
                        doorStates[key] = isOpen;
                    }
                }
            }
            else
            {
                foreach (var i in doorItems)
                {
                    int k = doorLevels.First(x => i.Title.StartsWith(x.Value)).Key;
                    doorStates[k] = false;
                    i.Title = $"{doorLevels[k]} - Closed";
                }
            }
        };



        // Create headlight submenu
        headlightSubMenu = new NativeMenu(
        "", // Title (empty or optional)
        "Xenon Kit", // Subtitle
        "", // ID or name
        new ScaledTexture(
            PointF.Empty,
            new SizeF(431, 107),
            "shopui_title_carmod", // GTA V workshop banner
            "shopui_title_carmod"
        )
        );
        pool.Add(headlightSubMenu);
        var headlightSubMenuItem = upgradeMenu.AddSubMenu(headlightSubMenu);

        // Headlight mod options
        var headlightModLevels = new Dictionary<int, string>
        {
            { 1, "Install Xenon Headlights - $2000" },
            { 2, "Remove Xenon Headlights - $500" }
        };

        // Store items to update their title later
        var headlightItems = new List<NativeItem>();

        // To store the original titles
        var originalHeadlightTitles = new Dictionary<NativeItem, string>();

        foreach (var level in headlightModLevels)
        {
            var headlightItem = new NativeItem(level.Value);

            // Store the original title
            originalHeadlightTitles[headlightItem] = headlightItem.Title;

            headlightItem.Activated += (sender, args) =>
            ToggleXenonHeadlights(level.Key, headlightItem, headlightItems, originalHeadlightTitles);


            headlightItems.Add(headlightItem);
            headlightSubMenu.Add(headlightItem);
        }

        headlightSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;

            if (vehicle != null && vehicle.Exists())
            {
                bool xenonEnabled = Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, vehicle, 22);

                foreach (var item in headlightItems)
                {
                    if (xenonEnabled && originalHeadlightTitles[item].StartsWith("Install"))
                    {
                        item.Title = "Installed";
                    }
                    else if (!xenonEnabled && originalHeadlightTitles[item].StartsWith("Remove"))
                    {
                        item.Title = "Removed";
                    }
                    else
                    {
                        item.Title = originalHeadlightTitles[item];
                    }
                }
            }
            else
            {
                // Reset to original titles if player is not in a vehicle
                foreach (var item in headlightItems)
                {
                    item.Title = originalHeadlightTitles[item];
                }
            }
        };



        // Neon submenu
        addNeonSubMenu = new NativeMenu(
            "",
            "Neon Kit",
            "",
            new ScaledTexture(PointF.Empty, new SizeF(431, 107), "shopui_title_carmod", "shopui_title_carmod")
        );
        pool.Add(addNeonSubMenu);
        var addNeonSubMenuItem = upgradeMenu.AddSubMenu(addNeonSubMenu);

        // Neon mod levels (Removed the "All" entry)
        var addNeonModLevels = new Dictionary<int, string>
        {
            { 1, "Front" },
            { 2, "Back" },
            { 3, "Left" },
            { 4, "Right" }
        };

        // Store items to update their title later
        var addNeonItems = new List<NativeItem>();

        // To store the original titles
        var originalAddNeonTitles = new Dictionary<NativeItem, string>();

        // Track whether the neon mod is installed
        var neonInstalled = new Dictionary<int, bool>();

        foreach (var level in addNeonModLevels)
        {
            var addNeonItem = new NativeItem($"{level.Value} - ${neonPrices[level.Key]}");

            // Store the original title
            originalAddNeonTitles[addNeonItem] = addNeonItem.Title;

            // Initialize the neonInstalled flag
            neonInstalled[level.Key] = false;

            addNeonItem.Activated += (sender, args) =>
            {
                // Toggle neon upgrade
                if (neonInstalled[level.Key])
                {
                    RemoveNeonUpgrade(level.Key); // Turn it off
                    neonInstalled[level.Key] = false;
                    addNeonItem.Title = originalAddNeonTitles[addNeonItem]; // Restore original title
                }
                else
                {
                    ApplyNeonUpgrade(level.Key); // Turn it on
                    neonInstalled[level.Key] = true;
                    addNeonItem.Title = "Installed";
                }

                // Reset other items
                foreach (var item in addNeonItems)
                {
                    if (item != addNeonItem)
                    {
                        item.Title = neonInstalled[addNeonModLevels.First(x => originalAddNeonTitles[item].StartsWith(x.Value)).Key]
                            ? "Installed"
                            : originalAddNeonTitles[item];
                    }
                }
            };

            addNeonItems.Add(addNeonItem);
            addNeonSubMenu.Add(addNeonItem);
        }

        addNeonSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;

            if (vehicle != null && vehicle.Exists())
            {
                // Check actual neon state on vehicle
                bool frontNeon = Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, vehicle, 2);
                bool backNeon = Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, vehicle, 3);
                bool leftNeon = Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, vehicle, 0);
                bool rightNeon = Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, vehicle, 1);

                foreach (var item in addNeonItems)
                {
                    if (item.Title.StartsWith("Front") || originalAddNeonTitles[item].StartsWith("Front"))
                        item.Title = frontNeon ? "Installed" : originalAddNeonTitles[item];
                    else if (item.Title.StartsWith("Back") || originalAddNeonTitles[item].StartsWith("Back"))
                        item.Title = backNeon ? "Installed" : originalAddNeonTitles[item];
                    else if (item.Title.StartsWith("Left") || originalAddNeonTitles[item].StartsWith("Left"))
                        item.Title = leftNeon ? "Installed" : originalAddNeonTitles[item];
                    else if (item.Title.StartsWith("Right") || originalAddNeonTitles[item].StartsWith("Right"))
                        item.Title = rightNeon ? "Installed" : originalAddNeonTitles[item];
                }
            }
            else
            {
                // Reset to original if not in vehicle
                foreach (var item in addNeonItems)
                {
                    item.Title = originalAddNeonTitles[item];
                }
            }
        };



        paintSubMenu = new NativeMenu(
        "",
        "Paint & Colors",
        "",
        new ScaledTexture(
            PointF.Empty,
            new SizeF(431, 107),
            "shopui_title_carmod",
            "shopui_title_carmod"
        )
        );
        pool.Add(paintSubMenu);
        var paintSubMenuItem = upgradeMenu.AddSubMenu(paintSubMenu);


        // Primary and Secondary Color Main Submenu
        NativeMenu primaryColorMenu = new NativeMenu(
            "",
            "Primary Color",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );

        NativeMenu secondaryColorMenu = new NativeMenu(
            "",
            "Secondary Color",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );

        NativeMenu interiorColorMenu = new NativeMenu(
            "",
            "Interior Color",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );

        NativeMenu wheelColorMenu = new NativeMenu(
            "",
            "Wheel Color",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );

        NativeMenu dialsColorMenu = new NativeMenu(
            "",
            "Dials Color",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );


        pool.Add(primaryColorMenu);
        pool.Add(secondaryColorMenu);
        pool.Add(interiorColorMenu);
        pool.Add(wheelColorMenu);
        pool.Add(dialsColorMenu);

        paintSubMenu.AddSubMenu(primaryColorMenu);
        paintSubMenu.AddSubMenu(secondaryColorMenu);
        paintSubMenu.AddSubMenu(interiorColorMenu);
        paintSubMenu.AddSubMenu(wheelColorMenu);
        paintSubMenu.AddSubMenu(dialsColorMenu);

        // Setup Menus
        SetupColorCategories(primaryColorMenu, ColorType.Primary);
        SetupColorCategories(secondaryColorMenu, ColorType.Secondary);
        SetupColorCategories(interiorColorMenu, ColorType.Interior);
        SetupColorCategories(wheelColorMenu, ColorType.Wheel);
        SetupColorCategories(dialsColorMenu, ColorType.Dials);

        // Tint Color Submenu
        tintColorSubMenu = new NativeMenu("", "Tint Colors", "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            ));
        pool.Add(tintColorSubMenu);
        paintSubMenu.AddSubMenu(tintColorSubMenu);
        tintColorSubMenu.Shown += (sender, args) =>
        {
            tintColorSubMenu.Clear();

            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle == null || !vehicle.Exists())
            {
                GTA.UI.Notification.Show("~r~You must be in a vehicle to view tint colors.");
                return;
            }

            for (int i = 0; i < tintlightColors.Count; i++)
            {
                int tintIndex = i;
                string tintName = tintlightColors[i];

                var tintItem = new NativeItem(tintName);
                tintItem.Activated += (s, e) =>
                {
                    Function.Call(Hash.SET_VEHICLE_WINDOW_TINT, vehicle, tintIndex);
                };

                tintColorSubMenu.Add(tintItem);
            }
        };

        // Headlight Color Submenu
        headlightColorSubMenu = new NativeMenu("", "Xenon Headlight Colors", "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            ));
        pool.Add(headlightColorSubMenu);
        paintSubMenu.AddSubMenu(headlightColorSubMenu);

        headlightColorSubMenu.Shown += (sender, args) =>
        {
            headlightColorSubMenu.Clear();

            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle == null || !vehicle.Exists())
            {
                GTA.UI.Notification.Show("~r~You must be in a vehicle to view headlight colors.");
                return;
            }

            // Check if Xenon headlights are enabled
            bool xenonEnabled = Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, vehicle, 22);
            if (!xenonEnabled)
            {
                GTA.UI.Notification.Show("~r~Install Xenon headlights first to use headlight colors!");
                return;
            }

            // Load color options
            for (int i = 0; i < headlightColors.Count; i++)
            {
                int colorIndex = i;
                string colorName = headlightColors[i];

                var headlightItem = new NativeItem(colorName);
                headlightItem.Activated += (s, e) =>
                {
                    Function.Call(Hash.SET_VEHICLE_XENON_LIGHT_COLOR_INDEX, vehicle, colorIndex);
                };

                headlightColorSubMenu.Add(headlightItem);
            }
        };

        // Neon Color Submenu
        neonColorSubMenu = new NativeMenu("", "Neon Light Color", "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            ));
        pool.Add(neonColorSubMenu);
        var neonSubMenuItem = paintSubMenu.AddSubMenu(neonColorSubMenu);

        // Define the neon colors (with the colors you want)
        Color[] colors = new Color[]
        {
    Color.AliceBlue, Color.AntiqueWhite, Color.Aqua, Color.Aquamarine, Color.Azure, Color.Beige,
    Color.Bisque, Color.BlanchedAlmond, Color.Blue, Color.BlueViolet, Color.Brown,
    Color.BurlyWood, Color.CadetBlue, Color.Chartreuse, Color.Chocolate, Color.Coral, Color.CornflowerBlue,
    Color.Cornsilk, Color.Crimson, Color.Cyan, Color.DarkBlue, Color.DarkCyan, Color.DarkGoldenrod,
    Color.DarkGray, Color.DarkGreen, Color.DarkKhaki, Color.DarkMagenta, Color.DarkOliveGreen, Color.DarkOrange,
    Color.DarkOrchid, Color.DarkRed, Color.DarkSalmon, Color.DarkSeaGreen, Color.DarkSlateBlue, Color.DarkSlateGray,
    Color.DarkTurquoise, Color.DarkViolet, Color.DeepPink, Color.DeepSkyBlue, Color.DimGray, Color.DodgerBlue,
    Color.Firebrick, Color.FloralWhite, Color.ForestGreen, Color.Fuchsia, Color.Gainsboro, Color.GhostWhite,
    Color.Gold, Color.Goldenrod, Color.Gray, Color.Green, Color.GreenYellow, Color.Honeydew, Color.HotPink,
    Color.IndianRed, Color.Indigo, Color.Ivory, Color.Khaki, Color.Lavender, Color.LavenderBlush, Color.LawnGreen,
    Color.LemonChiffon, Color.LightBlue, Color.LightCoral, Color.LightCyan, Color.LightGoldenrodYellow, Color.LightGray,
    Color.LightGreen, Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.LightSkyBlue, Color.LightSlateGray,
    Color.LightSteelBlue, Color.LightYellow, Color.Lime, Color.LimeGreen, Color.Linen, Color.Magenta, Color.Maroon,
    Color.MediumAquamarine, Color.MediumBlue, Color.MediumOrchid, Color.MediumPurple, Color.MediumSeaGreen,
    Color.MediumSlateBlue, Color.MediumSpringGreen, Color.MediumTurquoise, Color.MediumVioletRed, Color.MidnightBlue,
    Color.MintCream, Color.MistyRose, Color.Moccasin, Color.NavajoWhite, Color.Navy, Color.OldLace, Color.Olive,
    Color.OliveDrab, Color.Orange, Color.OrangeRed, Color.Orchid, Color.PaleGoldenrod, Color.PaleGreen, Color.PaleTurquoise,
    Color.PaleVioletRed, Color.PapayaWhip, Color.PeachPuff, Color.Peru, Color.Pink, Color.Plum, Color.PowderBlue,
    Color.Purple, Color.Red, Color.RosyBrown, Color.RoyalBlue, Color.SaddleBrown, Color.Salmon,
    Color.SandyBrown, Color.SeaGreen, Color.SeaShell, Color.Sienna, Color.Silver, Color.SkyBlue, Color.SlateBlue,
    Color.SlateGray, Color.Snow, Color.SpringGreen, Color.SteelBlue, Color.Tan, Color.Teal, Color.Thistle, Color.Tomato,
    Color.Transparent, Color.Turquoise, Color.Violet, Color.Wheat, Color.White, Color.WhiteSmoke, Color.Yellow,
    Color.YellowGreen
        };

        // Show neon color items when opening submenu
        neonColorSubMenu.Shown += (sender, args) =>
        {
            neonColorSubMenu.Clear();

            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle == null || !vehicle.Exists())
            {
                GTA.UI.Notification.Show("~r~You must be in a vehicle to use neon colors.");
                return;
            }

            // Check if neon lights are enabled on at least one side
            bool hasNeons = false;
            for (int i = 0; i < 4; i++) // 0 = left, 1 = right, 2 = front, 3 = back
            {
                if (Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, vehicle, i))
                {
                    hasNeons = true;
                    break;
                }
            }

            if (!hasNeons)
            {
                GTA.UI.Notification.Show("~r~Install and enable neon lights first!");
                return;
            }

            // Add items for each defined color
            for (int i = 0; i < colors.Length; i++)
            {
                int colorIndex = i;
                Color color = colors[i];
                string colorName = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(color.Name.Replace('_', ' '));

                var neonItem = new NativeItem(colorName);
                neonItem.Activated += (s, e) =>
                {
                    Function.Call(Hash.SET_VEHICLE_NEON_COLOUR, vehicle, color.R, color.G, color.B);
                };

                neonColorSubMenu.Add(neonItem);
            }
        };



        turboSubMenu = new NativeMenu(
            "", // Title (empty or optional)
            "Turbo", // Subtitle
            "", // ID or name
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod", // GTA V workshop banner
                "shopui_title_carmod"
            )
        );
        pool.Add(turboSubMenu);
        var turboSubMenuItem = upgradeMenu.AddSubMenu(turboSubMenu);

        // Turbo mod options
        var turboModLevels = new Dictionary<int, string>
        {
            { 1, "Install Turbo - 5000$" },
            { 2, "Remove Turbo - 500$" }
        };

        // Loop to create the items


        foreach (var level in turboModLevels)
        {
            var turboItem = new NativeItem(level.Value);
            turboItem.Activated += (sender, args) => ToggleTurbo(level.Key, turboItem);
            turboSubMenu.Add(turboItem);

            // Assign to class-level fields
            if (level.Key == 1) installTurboItem = turboItem;
            else if (level.Key == 2) removeTurboItem = turboItem;
        }

        turboSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;

            if (vehicle != null && vehicle.Exists())
            {
                bool turboEnabled = Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, vehicle, 18);

                if (turboEnabled)
                {
                    installTurboItem.Title = "Installed";
                    removeTurboItem.Title = "Remove Turbo - $500";
                }
                else
                {
                    installTurboItem.Title = "Install Turbo - $5000";
                    removeTurboItem.Title = "Removed";
                }
            }
            else
            {
                installTurboItem.Title = "Install Turbo - $5000";
                removeTurboItem.Title = "Remove Turbo - $500";
            }
        };


        

        // 1. Get current vehicle and available armor levels
        Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;


        armorSubMenu = new NativeMenu(
            "",
            "Armor",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );
        pool.Add(armorSubMenu);
        var armorSubMenuItem = upgradeMenu.AddSubMenu(armorSubMenu);

        // 3. Setup upgrade options
        var armorModLevels = new Dictionary<int, string>
        {
            { 1, "20% Armoured" },
            { 2, "40% Armoured" },
            { 3, "60% Armoured" },
            { 4, "80% Armoured" },
            { 5, "100% Armoured" }
        };

        var armorItems = new List<NativeItem>();
        var originalArmorTitles = new Dictionary<NativeItem, string>();

        foreach (var level in armorModLevels)
        {

                var armorItem = new NativeItem($"{level.Value} - ${armorPrices[level.Key]}");
                originalArmorTitles[armorItem] = armorItem.Title;

                armorItem.Activated += (sender, args) =>
                {
                    ApplyArmorUpgrade(level.Key);
                    armorItem.Title = "Installed";

                    // Reset all others
                    foreach (var item in armorItems)
                    {
                        if (item != armorItem)
                        {
                            item.Title = originalArmorTitles[item];
                        }
                    }
                };

                armorItems.Add(armorItem);
                armorSubMenu.Add(armorItem);

        }

        // 4. Update UI on menu shown
        armorSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle != null && vehicle.Exists())
            {
                int currentArmorLevel = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 16); // 0-based

                foreach (var item in armorItems)
                {
                    item.Title = originalArmorTitles[item];
                }

                foreach (var level in armorModLevels)
                {
                    var match = armorItems.FirstOrDefault(i => i.Title.StartsWith(level.Value));
                    if (match != null && currentArmorLevel == level.Key - 1)
                    {
                        match.Title = "Installed";
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in armorItems)
                {
                    item.Title = originalArmorTitles[item];
                }
            }
        };




        // 1. Create the Engine submenu
        engineSubMenu = new NativeMenu(
            "",
            "Engine",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );
        pool.Add(engineSubMenu);
        var engineSubMenuItem = upgradeMenu.AddSubMenu(engineSubMenu);

        // 2. Engine upgrade options
        var engineModLevels = new Dictionary<int, string>
        {
            { 1, "Basic Engine Upgrade" },
            { 2, "Improved Engine" },
            { 3, "High-End Engine" },
            { 4, "Race Engine" }
        };

        var engineItems = new List<NativeItem>();
        var originalEngineTitles = new Dictionary<NativeItem, string>();

        // 3. Populate menu items
        foreach (var level in engineModLevels)
        {
            var engineItem = new NativeItem($"{level.Value} - ${enginePrices[level.Key]}");
            originalEngineTitles[engineItem] = engineItem.Title;

            engineItem.Activated += (sender, args) =>
            {
                ApplyEngineUpgrade(level.Key);
                engineItem.Title = "Installed";

                foreach (var item in engineItems)
                {
                    if (item != engineItem)
                        item.Title = originalEngineTitles[item];
                }
            };

            engineItems.Add(engineItem);
            engineSubMenu.Add(engineItem);
        }

        // 4. Show event: Detect installed upgrade and update menu item titles
        engineSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle != null && vehicle.Exists())
            {
                int currentEngineLevel = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 11); // 11 = engine mod type

                foreach (var item in engineItems)
                    item.Title = originalEngineTitles[item];

                foreach (var level in engineModLevels)
                {
                    var match = engineItems.FirstOrDefault(i => i.Title.StartsWith(level.Value));
                    if (match != null && currentEngineLevel == level.Key - 1)
                    {
                        match.Title = "Installed";
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in engineItems)
                    item.Title = originalEngineTitles[item];
            }
        };


        // 1. Transmission submenu
        transmissionSubMenu = new NativeMenu(
            "", // Title (empty or optional)
            "Transmission", // Subtitle
            "", // ID or name
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod", // GTA V workshop banner
                "shopui_title_carmod"
            )
        );
        pool.Add(transmissionSubMenu);
        var transmissionSubMenuItem = upgradeMenu.AddSubMenu(transmissionSubMenu);

        // 2. Transmission mod levels
        var transmissionModLevels = new Dictionary<int, string>
        {
            { 1, "Street Transmission" },
            { 2, "Sport Transmission" },
            { 3, "Race Transmission" }
        };

        // Store items to update their title later
        var transmissionItems = new List<NativeItem>();
        var originalTransmissionTitles = new Dictionary<NativeItem, string>();

        foreach (var level in transmissionModLevels)
        {
            var transmissionItem = new NativeItem($"{level.Value} - ${transmissionPrices[level.Key]}");

            // Store the original title
            originalTransmissionTitles[transmissionItem] = transmissionItem.Title;

            transmissionItem.Activated += (sender, args) =>
            {
                // When activated, change the title to "Installed"
                transmissionItem.Title = "Installed";
                ApplyTransmissionUpgrade(level.Key); // Apply upgrade logic

                // Reset other item titles to their original state
                foreach (var item in transmissionItems)
                {
                    if (item != transmissionItem)
                    {
                        item.Title = originalTransmissionTitles[item];
                    }
                }
            };

            transmissionItems.Add(transmissionItem);
            transmissionSubMenu.Add(transmissionItem);
        }

        // 3. Show event: Detect installed upgrade and update menu item titles
        transmissionSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle != null && vehicle.Exists())
            {
                int currentTransmissionLevel = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 13); // 13 = transmission mod type

                foreach (var item in transmissionItems)
                    item.Title = originalTransmissionTitles[item];

                foreach (var level in transmissionModLevels)
                {
                    var match = transmissionItems.FirstOrDefault(i => i.Title.StartsWith(level.Value));
                    if (match != null && currentTransmissionLevel == level.Key - 1)
                    {
                        match.Title = "Installed";
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in transmissionItems)
                    item.Title = originalTransmissionTitles[item];
            }
        };

        // 4. Brake submenu
        brakeSubMenu = new NativeMenu(
            "", // Title (empty or optional)
            "Brakes", // Subtitle
            "", // ID or name
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod", // GTA V workshop banner
                "shopui_title_carmod"
            )
        );
        pool.Add(brakeSubMenu);
        var brakeSubMenuItem = upgradeMenu.AddSubMenu(brakeSubMenu);

        // 5. Brake mod levels
        var brakeModLevels = new Dictionary<int, string>
        {
            { 1, "Street Brakes" },
            { 2, "Sport Brakes" },
            { 3, "Race Brakes" }
        };

        // Store items to update their title later
        var brakeItems = new List<NativeItem>();
        var originalBrakeTitles = new Dictionary<NativeItem, string>();

        foreach (var level in brakeModLevels)
        {
            var brakeItem = new NativeItem($"{level.Value} - ${brakePrices[level.Key]}");

            // Store the original title
            originalBrakeTitles[brakeItem] = brakeItem.Title;

            brakeItem.Activated += (sender, args) =>
            {
                // When activated, change the title to "Installed"
                brakeItem.Title = "Installed";
                ApplyBrakeUpgrade(level.Key); // Apply upgrade logic

                // Reset other item titles to their original state
                foreach (var item in brakeItems)
                {
                    if (item != brakeItem)
                    {
                        item.Title = originalBrakeTitles[item];
                    }
                }
            };

            brakeItems.Add(brakeItem);
            brakeSubMenu.Add(brakeItem);
        }

        // 6. Show event: Detect installed upgrade and update menu item titles
        brakeSubMenu.Shown += (sender, args) =>
        {
            Vehicle vehicle = Game.Player.Character.CurrentVehicle;
            if (vehicle != null && vehicle.Exists())
            {
                int currentBrakeLevel = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 12); // 12 = brake mod type

                foreach (var item in brakeItems)
                    item.Title = originalBrakeTitles[item];

                foreach (var level in brakeModLevels)
                {
                    var match = brakeItems.FirstOrDefault(i => i.Title.StartsWith(level.Value));
                    if (match != null && currentBrakeLevel == level.Key - 1)
                    {
                        match.Title = "Installed";
                        break;
                    }
                }
            }
            else
            {
                foreach (var item in brakeItems)
                    item.Title = originalBrakeTitles[item];
            }
        };


        // Suspension submenu
        suspensionSubMenu = new NativeMenu(
        "", // Title (empty or optional)
        "Suspension", // Subtitle
        "", // ID or name
        new ScaledTexture(
            PointF.Empty,
            new SizeF(431, 107),
            "shopui_title_carmod", // GTA V workshop banner
            "shopui_title_carmod"
        )
        );
        pool.Add(suspensionSubMenu);
        var suspensionSubMenuItem = upgradeMenu.AddSubMenu(suspensionSubMenu);

        // Suspension mod levels, now ordered from Level 1 to Level 4
        var suspensionModLevels = new Dictionary<int, string>
        {
            { 1, "Suspension Level 1" },
            { 2, "Suspension Level 2" },
            { 3, "Suspension Level 3" },
            { 4, "Suspension Level 4" }
        };

        // Store items to update their title later
        var suspensionItems = new List<NativeItem>();

        // To store the original titles
        var originalSuspensionTitles = new Dictionary<NativeItem, string>();

        foreach (var level in suspensionModLevels)
        {
            var suspensionItem = new NativeItem($"{level.Value} - ${suspensionPrices[level.Key]}");

            // Store the original title
            originalSuspensionTitles[suspensionItem] = suspensionItem.Title;

            suspensionItem.Activated += (sender, args) =>
            {
                // When activated, change the title to "Installed"
                suspensionItem.Title = "Installed"; // Update title with "Installed"

                ApplySuspensionUpgrade(level.Key); // Apply upgrade logic

                // Reset other item titles to their original state
                foreach (var item in suspensionItems)
                {
                    if (item != suspensionItem)
                    {
                        item.Title = originalSuspensionTitles[item];  // Reset title
                    }
                }
            };

            suspensionItems.Add(suspensionItem);
            suspensionSubMenu.Add(suspensionItem);
        }



        // Create the "Wheels" submenu
        var wheelsSubMenu = new NativeMenu(
            "",
            "Wheels",
            "",
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );
        pool.Add(wheelsSubMenu);
        var wheelsSubMenuItem = upgradeMenu.AddSubMenu(wheelsSubMenu);
        // Define wheel categories and their max rim index
        var wheelCategories = new Dictionary<int, (string Name, int Max)>
        {
            { 0, ("Sport Wheels", 50) },
            { 1, ("Muscle Wheels", 40) },
            { 2, ("Lowrider Wheels", 30) },
            { 3, ("SUV Wheels", 35) },
            { 4, ("Offroad Wheels", 25) },
            { 5, ("Tuner Wheels", 45) },
            { 6, ("Bike Wheels", 20) },
            { 7, ("High End Wheels", 30) },
            { 8, ("Benny's Original Wheels", 30) },
            { 9, ("Benny's Bespoke Wheels", 30) },
            { 10, ("Open Wheels", 30) },
            { 11, ("Street Wheels", 30) },
            { 12, ("Track Wheels", 30) }
        };
        // Track current selected rim index (optional)
        Dictionary<int, int> currentWheelIndex = new Dictionary<int, int>();
        // Loop to add submenu per category
        // Global variables for preview cache
        // These should be declared at class level
    


        foreach (var category in wheelCategories)
        {
            int type = category.Key;
            string name = category.Value.Name;

            var categoryMenu = new NativeMenu("", name, "", new ScaledTexture(PointF.Empty, new SizeF(431, 107), "shopui_title_carmod", "shopui_title_carmod"));
            pool.Add(categoryMenu);
            wheelsSubMenu.AddSubMenu(categoryMenu);

            categoryMenu.Shown += (sender, args) =>
            {
                categoryMenu.Clear();

                Vehicle vehicle = Game.Player.Character.CurrentVehicle;
                if (vehicle == null || !vehicle.Exists()) return;

                // Save the original wheel type and rim index once
                if (!_hasCachedOriginal)
                {
                    _originalWheelType = Function.Call<int>(Hash.GET_VEHICLE_WHEEL_TYPE, vehicle);
                    _originalRimIndex = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 23);
                    _hasCachedOriginal = true;
                }

                // Set the current wheel type for this category
                Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, vehicle, type);

                int maxRims = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, vehicle, 23);
                List<NativeItem> rimItems = new List<NativeItem>();

                for (int i = 0; i < maxRims; i++)
                {
                    var rimItem = new NativeItem($"Rim Style {i}");
                    int rimIndex = i;

                    // On click: apply and charge
                    rimItem.Activated += (s, e) =>
                    {
                        ApplyWheel(type, rimIndex, 2000); // Final install
                    };

                    rimItems.Add(rimItem);
                    categoryMenu.Add(rimItem);
                }

                // Preview while scrolling
                categoryMenu.SelectedIndexChanged += (s, e) =>
                {
                    int index = categoryMenu.SelectedIndex;
                    if (index >= 0 && index < rimItems.Count)
                    {
                        PreviewWheel(type, index);
                    }
                };

                // Revert preview on exit if not purchased
                categoryMenu.Closed += (s, e) =>
                {
                    if (!_hasCachedOriginal) return;

                    Vehicle revertVehicle = Game.Player.Character.CurrentVehicle;
                    if (currentVehicle == null || !currentVehicle.Exists()) return;

                    Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, currentVehicle, _originalWheelType);
                    Function.Call(Hash.SET_VEHICLE_MOD, currentVehicle, 23, _originalRimIndex, false);

                    _hasCachedOriginal = false; // Clear cache
                };

            };
        }





        // Create Other Mods submenu
        var otherModsSubMenu = new NativeMenu(
            "", // Title (empty or optional)
            "Bodykits", // Subtitle
            "", // ID or name
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod", // GTA V workshop banner
                "shopui_title_carmod"
            )
        );
        pool.Add(otherModsSubMenu);
        var otherModsSubMenuItem = upgradeMenu.AddSubMenu(otherModsSubMenu);
        // Other mods mod options
        var otherModsModLevels = new Dictionary<int, string>
        {
            { 1, "Spoilers" },               // Spoilers
            { 2, "Front Bumper" },           // Front Bumper
            { 3, "Back Bumper" },            // Back Bumper
            { 4, "Side Skirt" },             // Side Skirt
            { 5, "Exhaust" },                // Exhaust
            { 6, "Frame" },                  // Frame
            { 7, "Grille" },                 // Grille
            { 8, "Hood" },                   // Hood
            { 9, "Fender" },                 // Fender
            { 10, "Right Fender" },          // Right Fender
            { 11, "Roof" },                  // Roof
            { 12, "Horn" },                  // Horn
            { 13, "Nitrous" },               // Nitrous
            { 14, "Subwoofer" },             // Subwoofer
            { 15, "Plate Holders" },         // Plate Holders
            { 16, "Plate Design" },          // Plate Design
            { 17, "Trim Design" },           // Trim Design
            { 18, "Ornaments" },             // Ornaments
            { 19, "Interior Upgrade" },     // Interior (Tofind)
            { 20, "Dial Design" },           // Dial Design
            { 21, "Interior Upgrade" },     // Interior (Tofind) (repeated)
            { 22, "Seats" },                 // Seats
            { 23, "Steering Wheel" },        // Steering Wheel
            { 24, "Knob" },                  // Knob
            { 25, "Ice" },                   // Ice
            { 26, "Trunk" },                 // Trunk
            { 27, "EngineBay1" },            // EngineBay1
            { 28, "EngineBay2" },            // EngineBay2
            { 29, "EngineBay3" },            // EngineBay3
            { 30, "Chassis2" },              // Chassis2
            { 31, "Chassis3" },              // Chassis3
            { 32, "Chassis4" },              // Chassis4
            { 33, "Chassis5" },              // Chassis5
            { 34, "Door_L" },                // Door_L
            { 35, "Door_R" },                // Door_R
            { 36, "Livery Mod" },            // Livery Mod
            { 37, "Lightbar" }               // Lightbar
        };
        // Add each mod item to the submenu based on availability
        foreach (var level in otherModsModLevels)
        {
            int modType = GetModType(level.Key);
            if (IsModAvailable(modType))
            {
                var otherModsItem = new NativeItem(level.Value);
                otherModsItem.Activated += (sender, args) => InstallVehicleMod(level.Key, otherModsItem);
                otherModsSubMenu.Add(otherModsItem);
            }
        }
        otherModsSubMenu.Shown += (sender, args) =>
        {
            otherModsSubMenu.Clear(); // Clear old items

            foreach (var level in otherModsModLevels)
            {
                int modType = GetModType(level.Key);
                if (IsModAvailable(modType))
                {
                    var otherModsItem = new NativeItem(level.Value);
                    otherModsItem.Activated += (s, a) => InstallVehicleMod(level.Key, otherModsItem);
                    otherModsSubMenu.Add(otherModsItem);
                }
            }
        };
    }



    private int notificationHandle = -1;

    private void OnTick(object sender, EventArgs e)
    {

        Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "shopui_title_carmod", true);
        Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "morelossantos");


        // Reset isNearUpgradeZone flag
        isNearUpgradeZone = false;

        // Check if the player is near any upgrade zone
        foreach (var zone in upgradeZonePositions)
        {
            if (Game.Player.Character.Position.DistanceTo(zone) < upgradeZoneRadius)
            {
                isNearUpgradeZone = true;
                break; // No need to check further once we find one zone
            }
        }

        // If the player is near any upgrade zone
        if (isNearUpgradeZone)
        {
            // Check if the player is in a vehicle
            if (Game.Player.Character.IsInVehicle())
            {
                string buttonName = enable.ToString();
                // Show the workshop message if the player is in a vehicle and the notification is not already shown
                if (notificationHandle == -1)
                {
                    notificationHandle = GTA.UI.Notification.Show($"~w~Welcome to the Workshop!~w~ Press ~b~{buttonName}~w~ to modify your car.");
                }
            }
            else
            {
                // Show a message telling the player they must be in a vehicle to access the workshop
                if (notificationHandle == -1)
                {
                  
                }
            }
        }
        else
        {
            // If the player is no longer in any upgrade zone, hide the notification
            if (notificationHandle != -1)
            {
                GTA.UI.Notification.Hide(notificationHandle); // Hide the notification using the handle
                notificationHandle = -1; // Reset the handle to indicate no active notification
            }
        }

        // Process LemonUI menu pool
        pool.Process();
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        // Debugging output: Check if the player is in a vehicle and if the key is being pressed
        if (Game.Player.Character.IsInVehicle())
        {
            if (isNearUpgradeZone && e.KeyCode == enable)
            {
                // Toggle the upgrade menu visibility
                upgradeMenu.Visible = !upgradeMenu.Visible;
            }
        }
        else
        {
           
        }
    }
    // Blip Stuff
    private void LoadUpgradeZonePositions()
    {
        int zoneIndex = 1;

        while (true)
        {
            // Load the coordinates for each zone
            float x = config.GetValue<float>($"UpgradeZone{zoneIndex}", "LocationX", float.NaN);
            float y = config.GetValue<float>($"UpgradeZone{zoneIndex}", "LocationY", float.NaN);
            float z = config.GetValue<float>($"UpgradeZone{zoneIndex}", "LocationZ", float.NaN);

            // Stop if any value is NaN
            if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z))
            {
                break; // No more upgrade zones
            }

            // Add the location to the list
            upgradeZonePositions.Add(new Vector3(x, y, z));

            zoneIndex++;
        }
    }


    private void CreateUpgradeZoneBlips(string sprite, string color, string name)
    {

        for (int i = 0; i < upgradeZonePositions.Count; i++)
        {

            // If you want to merge the zones into a single blip, use the first zone position or average position
            Vector3 centralPosition = upgradeZonePositions[i];  // Use the first upgrade zone position

            // Create a single blip for all zones
            Blip blip = World.CreateBlip(centralPosition);

            // Set the properties of the blip
            blip.Sprite = GetBlipSprite(sprite);  // Set sprite based on the sprite string
            blip.Color = GetBlipColor(color);  // Set color based on the color string
            blip.Name = $"{name}";  // Set the name for the blip (no need for numbering)

            // Add the created blip to the list of upgrade zone blips
            upgradeZoneBlips.Clear();  // Remove any existing blips to avoid clutter
            upgradeZoneBlips.Add(blip);
        }
    }
    private BlipSprite GetBlipSprite(string spriteString)
    {
        switch (spriteString.ToLower())
        {
            case "losantoscustoms":
                return BlipSprite.LosSantosCustoms;
            case "garage":
                return BlipSprite.Garage;
            case "carwash":
                return BlipSprite.CarWash;
            case "bennys":
                return BlipSprite.Bennys;
            case "autoshop":
                return BlipSprite.AutoShopProperty;
            default:
                return BlipSprite.Standard; // Default sprite if not matched
        }
    }
    private BlipColor GetBlipColor(string colorString)
    {
        switch (colorString.ToLower())
        {
            case "white":
                return BlipColor.White;
            case "red":
                return BlipColor.Red;
            case "green":
                return BlipColor.Green;
            case "blue":
                return BlipColor.Blue;
            case "yellow":
                return BlipColor.Yellow;
            case "whitenotpure":
                return BlipColor.WhiteNotPure;
            case "yellow2":
                return BlipColor.Yellow2;
            case "greydark":
                return BlipColor.GreyDark;
            case "redlight":
                return BlipColor.RedLight;
            case "purple":
                return BlipColor.Purple;
            case "orange":
                return BlipColor.Orange;
            case "greendark":
                return BlipColor.GreenDark;
            case "bluelight":
                return BlipColor.BlueLight;
            case "bluedark":
                return BlipColor.BlueDark;
            case "grey":
                return BlipColor.Grey;
            case "yellowdark":
                return BlipColor.YellowDark;
            case "pink":
                return BlipColor.Pink;
            case "greylight":
                return BlipColor.GreyLight;
            case "blue3":
                return BlipColor.Blue3;
            case "blue4":
                return BlipColor.Blue4;
            case "green2":
                return BlipColor.Green2;
            case "yellow4":
                return BlipColor.Yellow4;
            case "yellow5":
                return BlipColor.Yellow5;
            case "white2":
                return BlipColor.White2;
            case "yellow6":
                return BlipColor.Yellow6;
            case "blue5":
                return BlipColor.Blue5;
            case "red4":
                return BlipColor.Red4;
            case "reddark":
                return BlipColor.RedDark;
            case "blue6":
                return BlipColor.Blue6;
            case "bluedark2":
                return BlipColor.BlueDark2;
            case "reddark2":
                return BlipColor.RedDark2;
            case "menuyellow":
                return BlipColor.MenuYellow;
            case "blue7":
                return BlipColor.Blue7;
            default:
                return BlipColor.BlueLight; // Default color if not matched
        }
    }


    //Color Upgrades


    private static int currentColorIndex = 0;  // Track the current index in the color list
    // Enum for Xenon tintlight Colors (Matching with your existing colors)
    private enum TintColors
    {
        None = 0,
        Black = 1,
        DarkSmoke = 2,
        LightSmoke = 3,
        Stock = 4,
        Limo = 5,
        Green = 6,
    }
    // List of Xenon tintlight Colors
    private List<string> tintlightColors = new List<string>
    {
        "None", "Black", "DarkSmoke", "LightSmoke", "Stock", "Limo", "Green"
    };

    // Enum for Xenon Headlight Colors (Matching with your existing colors)
    private enum HeadlightColors
    {
        White = 0,
        Blue = 1,
        LightBlue = 2,
        Green = 3,
        LightGreen = 4,
        LightYellow = 5,
        Yellow = 6,
        Orange = 7,
        Red = 8,
        LightPink = 9,
        Pink = 10,
        Purple = 11,
        LightPurple = 12
    }
    // List of Xenon Headlight Colors
    private List<string> headlightColors = new List<string>
{
    "White", "Blue", "Light Blue", "Green", "Light Green", "Light Yellow", "Yellow", "Orange", "Red", "Light Pink", "Pink", "Purple", "Light Purple"
};
    // Function to apply Xenon Headlight color

    enum ColorType
    {
        Primary,
        Secondary,
        Interior,
        Wheel,
        Dials
    }

    // ==============================
    //    Create Category SubMenu
    // ==============================

    NativeMenu CreateCategorySubMenu(string title, string subtitle, bool showMessage = false)
    {
        var message = showMessage ? "Add Pearlescent first to get its effect" : "";  // Only show the message if required
        var menu = new NativeMenu(
            "",
            title,
            message,  // Set subtitle message conditionally
            new ScaledTexture(
                PointF.Empty,
                new SizeF(431, 107),
                "shopui_title_carmod",
                "shopui_title_carmod"
            )
        );
        pool.Add(menu);
        return menu;
    }

    // ==============================
    //    Setup Categories
    // ==============================

    void SetupColorCategories(NativeMenu parentMenu, ColorType type)
    {
        if (type == ColorType.Interior || type == ColorType.Wheel || type == ColorType.Dials)
        {
            // No Pearlescent message for Interior, Wheel, and Dials
            var metallicMenu = CreateCategorySubMenu("Metallic Colors", "Metallic", false);  // No message here
            parentMenu.AddSubMenu(metallicMenu);

            parentMenu.Shown += (sender, args) =>
            {
                AddCategoryMenuItems(metallicMenu, VehicleColors.MetallicColors, type);
            };
        }
        else
        {
            // Show Pearlescent message for Primary and Secondary
            var metallicMenu = CreateCategorySubMenu("Metallic Colors", "Metallic", true);  // Show message here
            var matteMenu = CreateCategorySubMenu("Matte Colors", "Matte", true);  // Show message here
            var metalsMenu = CreateCategorySubMenu("Metal Colors", "Metals", true);  // Show message here
            var chromeMenu = CreateCategorySubMenu("Chrome Colors", "Chrome", true);  // Show message here
            var pearlescentMenu = CreateCategorySubMenu("Pearlescent Colors", "Pearlescent", true);  // Show message here

            parentMenu.AddSubMenu(metallicMenu);
            parentMenu.AddSubMenu(matteMenu);
            parentMenu.AddSubMenu(metalsMenu);
            parentMenu.AddSubMenu(chromeMenu);
            parentMenu.AddSubMenu(pearlescentMenu);

            parentMenu.Shown += (sender, args) =>
            {
                AddCategoryMenuItems(metallicMenu, VehicleColors.MetallicColors, type);
                AddCategoryMenuItems(matteMenu, VehicleColors.MatteColors, type);
                AddCategoryMenuItems(chromeMenu, VehicleColors.ChromeColors, type);
                AddCategoryMenuItems(metalsMenu, VehicleColors.Metals, type);
                AddCategoryMenuItems(pearlescentMenu, VehicleColors.PearlescentColors, type);
            };
        }
    }

    // ==============================
    //    Add Color Items
    // ==============================

    void AddCategoryMenuItems(NativeMenu menu, Dictionary<string, VehicleColors.VehicleColorInfo> colorDict, ColorType type)
    {
        menu.Clear();
        // Convert the dictionary to a list for index-based access
        var colorList = colorDict.ToList();

        // Add items and Activated event
        for (int i = 0; i < colorList.Count; i++)
        {
            var color = colorList[i];
            var item = new NativeItem(color.Key);
            menu.Add(item);

            // Apply permanently on click
            item.Activated += (sender, e) =>
            {
                Vehicle veh = Game.Player.Character.CurrentVehicle;
                if (veh != null && veh.Exists())
                {
                    ApplyColor(veh, type, color.Value.Color);

                    // Pearlescent special case
                    if (type == ColorType.Primary && colorDict == VehicleColors.PearlescentColors)
                    {
                        SetPearlescentEffect(veh, color.Key);
                    }
                }
            };
        }

        // Preview on selection change
        menu.SelectedIndexChanged += (s, e) =>
        {
            Vehicle veh = Game.Player.Character.CurrentVehicle;
            if (veh == null || !veh.Exists()) return;
            int idx = menu.SelectedIndex;
            if (idx < 0 || idx >= colorList.Count) return;

            var previewColor = colorList[idx];
            ApplyColor(veh, type, previewColor.Value.Color);

            // Pearlescent preview
            if (type == ColorType.Primary && colorDict == VehicleColors.PearlescentColors)
            {
                SetPearlescentEffect(veh, previewColor.Key);
            }
        };
    }

    void ApplyColor(Vehicle veh, ColorType type, int colorId)
    {
        switch (type)
        {
            case ColorType.Primary:
                veh.Mods.PrimaryColor = (VehicleColor)colorId;
                break;
            case ColorType.Secondary:
                veh.Mods.SecondaryColor = (VehicleColor)colorId;
                break;
            case ColorType.Interior:
                veh.Mods.TrimColor = (VehicleColor)colorId;
                break;
            case ColorType.Wheel:
                veh.Mods.RimColor = (VehicleColor)colorId;
                break;
            case ColorType.Dials:
                veh.Mods.DashboardColor = (VehicleColor)colorId;
                break;
        }
    }
    // ==============================
    //    Pearlescent Helper
    // ==============================

    void SetPearlescentEffect(Vehicle vehicle, string pearlescentColor)
    {
        if (vehicle == null || !vehicle.Exists())
            return;

        // Get the pearlescent color ID
        int pearlescentColorID = VehicleColors.PearlescentColors.ContainsKey(pearlescentColor)
            ? VehicleColors.PearlescentColors[pearlescentColor].Color
            : 0;

        // Apply only the pearlescent color without touching the base colors
        vehicle.Mods.PearlescentColor = (VehicleColor)pearlescentColorID;

    }









    // Modifications
    private bool IsModAvailable(int modType)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;

        if (vehicle == null || !vehicle.Exists())
            return false;

        // This gets how many valid mods exist for this mod slot
        int modCount = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, vehicle, modType);

        // If modCount is 0, it means the vehicle doesn't support this mod type
        if (modCount <= 0)
            return false;

        // Optionally skip wings/fenders on motorcycles, bikes, etc.
        Model model = vehicle.Model;
        if (model.IsBike || model.IsBicycle)
        {
            // Skip specific mod types that bikes never support
            if (modType == 8 || modType == 9 || modType == 10) // Fenders, right fender, roof
                return false;
        }

        return true;
    }
    private void InstallVehicleMod(int modIndex, NativeItem modItem)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;

        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to install mods!");
            return;
        }

        try
        {
            // Determine mod type based on the selected index
            int modType = GetModType(modIndex);

            // Check how many mods are installed and install the new mod
            int modCount = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, modType); // Get the current mod count
            modCount++; // Increment mod count

            // Install the new mod by setting a new mod with the incremented mod count
            Function.Call(Hash.SET_VEHICLE_MOD, vehicle, modType, modCount);

            // Optionally update the menu item text or handle other UI feedback
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }
    // Get the mod type based on the mod index
    private int GetModType(int modIndex)
    {
        switch (modIndex)
        {
            case 1: return 0;  // Spoilers
            case 2: return 1;  // Front Bumper
            case 3: return 2;  // Back Bumper
            case 4: return 3;  // Side Skirt
            case 5: return 4;  // Exhaust
            case 6: return 5;  // Frame
            case 7: return 6;  // Grille
            case 8: return 7;  // Hood
            case 9: return 8;  // Fender
            case 10: return 9; // Right Fender
            case 11: return 10; // Roof
            case 12: return 14; // Horn
            case 13: return 17; // Nitrous
            case 14: return 19; // Subwoofer
            case 15: return 25; // Plate Holders
            case 16: return 26; // Plate Design
            case 17: return 27; // Trim Design
            case 18: return 28; // Ornaments
            case 19: return 29; // Interior (Tofind)
            case 20: return 30; // Dial Design
            case 21: return 31; // Interior (Tofind) (repeated)
            case 22: return 32; // Seats
            case 23: return 33; // Steering Wheel
            case 24: return 34; // Knob
            case 25: return 36; // Ice
            case 26: return 37; // Trunk
            case 27: return 39; // EngineBay1
            case 28: return 40; // EngineBay2
            case 29: return 41; // EngineBay3
            case 30: return 42; // Chassis2
            case 31: return 43; // Chassis3
            case 32: return 44; // Chassis4
            case 33: return 45; // Chassis5
            case 34: return 46; // Door_L
            case 35: return 47; // Door_R
            case 36: return 48; // Livery Mod
            case 37: return 49; // Lightbar
            default: return 0;  // Default to Spoilers if invalid index
        }
    }
    private void RepairCar()
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to repair it!");
            return;
        }

        float bodyHealth = vehicle.BodyHealth;
        float engineHealth = vehicle.EngineHealth;
        float tankHealth = vehicle.PetrolTankHealth;

        // Detect damage
        bool isDamaged =
            bodyHealth < 1000f ||
            engineHealth < 1000f ||
            tankHealth < 1000f ||
            vehicle.IsDamaged ||
            !vehicle.IsDriveable;

        if (!isDamaged)
        {
            GTA.UI.Notification.Show("~g~Your vehicle is already in perfect condition. No repair needed.");
            return;
        }

        // Total possible missing health: 3000
        float totalMissingHealth = (1000f - bodyHealth) + (1000f - engineHealth) + (1000f - tankHealth);
        float damageRatio = totalMissingHealth / 3000f; // 0.0 to 1.0

        // Sensitively scale cost from $500 to $5000
        int minCost = 500;
        int maxCost = 5000;
        int repairCost = (int)(minCost + (damageRatio * (maxCost - minCost)));

        int playerMoney = Game.Player.Money;

        if (playerMoney < repairCost)
        {
            GTA.UI.Notification.Show("~r~You do not have enough money to repair your vehicle!");
            return;
        }

        try
        {
            Game.Player.Money -= repairCost;
            vehicle.Repair();
            vehicle.Wash();

            GTA.UI.Notification.Show($"~g~Vehicle repaired! ~s~Cost: ~y~${repairCost}");
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }
    private void ToggleTurbo(int level, NativeItem turboItem)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;

        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to install/remove turbos!");
            return;
        }

        // Initial check for turbo mod state
        bool turboEnabled = Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, vehicle, 18);

        int upgradeCost = TurboPrices[level];
        int playerMoney = Game.Player.Money;

        if (level == 1 && turboEnabled)
        {
            GTA.UI.Notification.Show("~g~Turbo is already installed.");
            return;
        }
        else if (level == 2 && !turboEnabled)
        {
            GTA.UI.Notification.Show("~y~Turbo is already removed.");
            return;
        }

        if (playerMoney < upgradeCost)
        {
            GTA.UI.Notification.Show($"~r~You do not have enough money! Required: ${upgradeCost}");
            return;
        }

        try
        {
            Game.Player.Money -= upgradeCost;

            if (level == 1 && !turboEnabled)  // Install Turbo
            {
                Function.Call(Hash.TOGGLE_VEHICLE_MOD, vehicle, 18, true);

                // Confirm if turbo was actually enabled
                if (Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, vehicle, 18))
                {

                    installTurboItem.Title = "Installed";
                    removeTurboItem.Title = "Remove Turbo - $500";
                }
                else
                {
                    GTA.UI.Notification.Show("~r~Turbo installation failed!");
                }
            }
            else if (level == 2 && turboEnabled)  // Remove Turbo
            {
                Function.Call(Hash.TOGGLE_VEHICLE_MOD, vehicle, 18, false);

                // Confirm if turbo was actually disabled
                if (!Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, vehicle, 18))
                {

                    installTurboItem.Title = "Install Turbo - $5000";
                    removeTurboItem.Title = "Removed";
                }
                else
                {
                    GTA.UI.Notification.Show("~r~Turbo removal failed!");
                }
            }
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }
    // Function to toggle Xenon headlights
    private void ToggleXenonHeadlights(int level, NativeItem triggeredItem, List<NativeItem> allItems, Dictionary<NativeItem, string> originalTitles)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;

        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to toggle headlights!");
            return;
        }

        bool xenonEnabled = Function.Call<bool>(Hash.IS_TOGGLE_MOD_ON, vehicle, 22);
        int upgradeCost = headlightPrices[level];
        int playerMoney = Game.Player.Money;

        // Exit early if already in desired state
        if ((level == 1 && xenonEnabled) || (level == 2 && !xenonEnabled))
        {
            GTA.UI.Notification.Show("~y~No changes made.");
            return;
        }

        if (playerMoney < upgradeCost)
        {
            GTA.UI.Notification.Show($"~r~Not enough money! Required: ${upgradeCost}");
            return;
        }

        try
        {
            // Deduct money
            Game.Player.Money -= upgradeCost;

            if (level == 1) // Install
            {
                Function.Call(Hash.TOGGLE_VEHICLE_MOD, vehicle, 22, true);

            }
            else if (level == 2) // Remove
            {
                Function.Call(Hash.TOGGLE_VEHICLE_MOD, vehicle, 22, false);

            }

            // Update all menu item titles
            foreach (var item in allItems)
            {
                if (item == triggeredItem)
                {
                    item.Title = level == 1 ? "Installed" : "Removed";
                }
                else
                {
                    item.Title = originalTitles[item]; // Reset the other one
                }
            }
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }
    private void ApplyTransmissionUpgrade(int level)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to upgrade!");
            return;
        }

        // Define multipliers for engine torque based on the transmission upgrade level
        var transmissionTorqueMultipliers = new Dictionary<int, float>
    {
        { 1, 1.05f },  // Basic Transmission - 5% torque increase
        { 2, 1.15f },  // Improved Transmission - 15% torque increase
        { 3, 1.25f },  // High-End Transmission - 25% torque increase
        { 4, 1.40f }   // Race Transmission - 40% torque increase
    };

        // Get the appropriate torque multiplier based on the level
        float torqueMultiplier = transmissionTorqueMultipliers.ContainsKey(level) ? transmissionTorqueMultipliers[level] : 1.0f;

        // Apply the torque multiplier to the vehicle's engine
        vehicle.EngineTorqueMultiplier = torqueMultiplier;

        // Log for debugging purposes (optional)
        Console.WriteLine($"Engine Torque Multiplier for Transmission Level {level}: {torqueMultiplier}");
    }

 
    Dictionary<int, int> currentWheelIndex = new Dictionary<int, int>();


    private void PreviewWheel(int wheelType, int rimIndex)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists()) return;

        Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, vehicle, wheelType);
        Function.Call(Hash.SET_VEHICLE_MOD, vehicle, 23, rimIndex, false);
    }

    private void ApplyWheel(int wheelType, int rimIndex, int cost)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to install wheels!");
            return;
        }

        if (Game.Player.Money < cost)
        {
            GTA.UI.Notification.Show($"~r~You need ${cost} to buy this rim.");
            return;
        }

        try
        {
            Game.Player.Money -= cost;
            GTA.UI.Notification.Show($"~g~Rim installed for ${cost}.");

            Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, vehicle, wheelType);
            Function.Call(Hash.SET_VEHICLE_MOD, vehicle, 23, rimIndex, false);

            _hasCachedOriginal = false; // Clear cache after confirmed purchase
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~Failed to apply wheels: " + ex.Message);
        }
    }
    private void ApplyBrakeUpgrade(int level)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to upgrade!");
            return;
        }

        // Check the current brake level
        int currentBrakeLevel = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 12); // 12 is the brake mod type
        if (currentBrakeLevel == level - 1)  // If the brakes are already at the selected level
        {
            GTA.UI.Notification.Show($"~g~Brakes are already at Level {level}. No upgrade needed.");
            return; // No charge, exit early
        }

        // Check if the player has enough money for the upgrade
        int playerMoney = Game.Player.Money;
        int upgradeCost = brakePrices[level]; // Get the cost for the selected brake level

        if (playerMoney < upgradeCost)
        {
            GTA.UI.Notification.Show("~r~You do not have enough money for this upgrade!");
            return;
        }

        try
        {
            // Deduct the money for the upgrade
            Game.Player.Money -= upgradeCost;

            // Set brake mod using the level selected
            Function.Call(Hash.SET_VEHICLE_MOD, vehicle, 12, level - 1, false);
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }
    // Method to apply neon upgrade
    private void ApplyNeonUpgrade(int level)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to upgrade!");
            return;
        }

        try
        {
            // Check if player has enough money
            int playerMoney = Game.Player.Money;
            int upgradeCost = neonPrices[level];

            if (playerMoney < upgradeCost)
            {
                GTA.UI.Notification.Show("~r~Not enough money for this upgrade!");
                return;
            }

            // Deduct money
            Game.Player.Money -= upgradeCost;

            // Toggle neon based on level
            switch (level)
            {
                case 1: // Front Neon
                    ToggleNeon(vehicle, 2); // Toggle front neon
                    break;
                case 2: // Back Neon
                    ToggleNeon(vehicle, 3); // Toggle back neon
                    break;
                case 3: // Left Neon
                    ToggleNeon(vehicle, 0); // Toggle left neon
                    break;
                case 4: // Right Neon
                    ToggleNeon(vehicle, 1); // Toggle right neon
                    break;
            }

        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }
    // Method to toggle a single neon light
    private void ToggleNeon(Vehicle vehicle, int neonIndex)
    {
        bool isNeonOn = Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, vehicle, neonIndex);
        Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, vehicle, neonIndex, !isNeonOn); // Toggle neon
    }
    // Method to remove neon upgrade (turn off neon lights)

    // Method to remove neon upgrade (turn off neon lights)
    private void RemoveNeonUpgrade(int level)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to uninstall the upgrade!");
            return;
        }

        switch (level)
        {
            case 1: Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, vehicle, 2, false); break; // Front
            case 2: Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, vehicle, 3, false); break; // Back
            case 3: Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, vehicle, 0, false); break; // Left
            case 4: Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, vehicle, 1, false); break; // Right
        }

        GTA.UI.Notification.Show("~r~Neon removed.");
    }
    private void ToggleDoor(int level, bool open)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists()) return;

        try
        {
            string doorName = "Unknown";

            if (level == 7) // All doors
            {
                for (int i = 0; i <= 5; i++)
                    Function.Call(open ? Hash.SET_VEHICLE_DOOR_OPEN : Hash.SET_VEHICLE_DOOR_SHUT, vehicle, i, false, false);

                doorName = "All doors";
            }
            else
            {
                int doorIndex = -1;

                switch (level)
                {
                    case 1: doorIndex = 4; doorName = "Bonnet"; break;
                    case 2: doorIndex = 5; doorName = "Trunk"; break;
                    case 3: doorIndex = 0; doorName = "Left Front Door"; break;
                    case 4: doorIndex = 1; doorName = "Right Front Door"; break;
                    case 5: doorIndex = 2; doorName = "Left Rear Door"; break;
                    case 6: doorIndex = 3; doorName = "Right Rear Door"; break;
                }

                if (doorIndex >= 0)
                {
                    Function.Call(open ? Hash.SET_VEHICLE_DOOR_OPEN : Hash.SET_VEHICLE_DOOR_SHUT, vehicle, doorIndex, false, false);
                }
            }
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }

    // Method to apply armor upgrade
    private void ApplyArmorUpgrade(int level)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to upgrade!");
            return;
        }

        int currentArmorLevel = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 16);
        if (currentArmorLevel == level - 1)
        {
            GTA.UI.Notification.Show($"~g~Armor is already at Level {level}.");
            return;
        }

        int playerMoney = Game.Player.Money;
        int upgradeCost = armorPrices[level];

        if (playerMoney < upgradeCost)
        {
            GTA.UI.Notification.Show("~r~Not enough money!");
            return;
        }

        try
        {
            Game.Player.Money -= upgradeCost;
            Function.Call(Hash.SET_VEHICLE_MOD, vehicle, 16, level - 1, false);
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~Error: " + ex.Message);
        }
    }
    private void ApplyEngineUpgrade(int level)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to upgrade!");
            return;
        }

        // Define multipliers for engine power based on the engine upgrade level
        var enginePowerMultipliers = new Dictionary<int, float>
    {
        { 1, 1.10f },  // Basic Engine - 10% power increase
        { 2, 1.25f },  // Improved Engine - 25% power increase
        { 3, 1.50f },  // High-End Engine - 50% power increase
        { 4, 1.75f }   // Race Engine - 75% power increase
    };

        // Get the appropriate power multiplier based on the level
        float powerMultiplier = enginePowerMultipliers.ContainsKey(level) ? enginePowerMultipliers[level] : 1.0f;

        // Apply the power multiplier to the vehicle's engine
        vehicle.EnginePowerMultiplier = powerMultiplier;

        // Log for debugging purposes (optional)
        Console.WriteLine($"Engine Power Multiplier for Level {level}: {powerMultiplier}");
    }
    // Method to apply acceleration changes based on the engine upgrade level



    private void ApplySuspensionUpgrade(int level)
    {
        Vehicle vehicle = Game.Player.Character.CurrentVehicle;
        if (vehicle == null || !vehicle.Exists())
        {
            GTA.UI.Notification.Show("~r~You must be in a vehicle to upgrade!");
            return;
        }

        // Check the current suspension level
        int currentSuspensionLevel = Function.Call<int>(Hash.GET_VEHICLE_MOD, vehicle, 15); // 15 is the suspension mod type
        if (currentSuspensionLevel == level - 1)  // If the suspension is already at the selected level
        {
            GTA.UI.Notification.Show($"~g~Suspension is already at Level {level}. No upgrade needed.");
            return; // No charge, exit early
        }

        // Check if the player has enough money for the upgrade
        int playerMoney = Game.Player.Money;
        int upgradeCost = suspensionPrices[level]; // Get the cost for the selected suspension level

        if (playerMoney < upgradeCost)
        {
            GTA.UI.Notification.Show("~r~You do not have enough money for this upgrade!");
            return;
        }

        try
        {
            // Deduct the money for the upgrade
            Game.Player.Money -= upgradeCost;

            // Set suspension mod using the level selected
            Function.Call(Hash.SET_VEHICLE_MOD, vehicle, 15, level - 1, false);
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show("~r~An error occurred: " + ex.Message);
        }
    }
}
