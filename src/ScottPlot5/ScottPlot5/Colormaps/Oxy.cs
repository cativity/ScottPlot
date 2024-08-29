﻿//This is a cmocean colormap
//All credit to Kristen Thyng
//This colormap is under the MIT License
//All cmocean colormaps are available at https://github.com/matplotlib/cmocean/tree/master/cmocean/rgb

namespace ScottPlot.Colormaps;

public class Oxy : ByteColormapBase
{
    public override string Name => "Oxy";

    public override (byte r, byte g, byte b)[] Rgbs => _rgbs;

    private static readonly (byte, byte, byte)[] _rgbs =
    [
        (64, 5, 5), (65, 5, 5), (67, 6, 6), (68, 6, 6), (71, 6, 7), (72, 6, 7), (73, 6, 7), (75, 6, 8), (77, 7, 8), (79, 7, 9), (80, 7, 9), (81, 7, 9),
        (84, 7, 10), (85, 7, 11), (87, 7, 11), (88, 7, 11), (91, 7, 12), (92, 7, 12), (93, 7, 12), (95, 7, 13), (98, 7, 13), (99, 7, 14), (100, 7, 14),
        (102, 7, 14), (104, 7, 14), (106, 6, 15), (107, 6, 15), (109, 6, 15), (111, 6, 15), (113, 6, 15), (114, 6, 15), (115, 5, 15), (118, 5, 15),
        (120, 5, 15), (121, 5, 15), (122, 5, 15), (125, 5, 14), (126, 5, 14), (127, 6, 13), (129, 6, 13), (131, 8, 12), (132, 9, 12), (133, 10, 11),
        (134, 12, 11), (136, 14, 10), (137, 16, 10), (138, 17, 9), (139, 19, 9), (141, 21, 8), (142, 23, 8), (143, 24, 8), (80, 79, 79), (80, 80, 80),
        (81, 81, 80), (82, 81, 81), (83, 83, 83), (84, 84, 83), (85, 84, 84), (86, 85, 85), (87, 87, 86), (88, 87, 87), (89, 88, 88), (89, 89, 88),
        (91, 90, 90), (92, 91, 91), (92, 92, 91), (93, 93, 92), (95, 94, 94), (95, 95, 94), (96, 96, 95), (97, 96, 96), (98, 98, 97), (99, 99, 98),
        (100, 99, 99), (101, 100, 100), (102, 102, 101), (103, 102, 102), (104, 103, 103), (104, 104, 103), (106, 105, 105), (107, 106, 106),
        (107, 107, 106), (108, 108, 107), (110, 109, 109), (111, 110, 110), (111, 111, 110), (112, 112, 111), (114, 113, 113), (114, 114, 113),
        (115, 115, 114), (116, 115, 115), (118, 117, 116), (118, 118, 117), (119, 119, 118), (120, 119, 119), (121, 121, 120), (122, 122, 121),
        (123, 123, 122), (124, 123, 123), (125, 125, 124), (126, 126, 125), (127, 127, 126), (129, 128, 127), (129, 129, 128), (130, 130, 129),
        (131, 131, 130), (133, 132, 131), (133, 133, 132), (134, 134, 133), (135, 135, 134), (137, 136, 135), (137, 137, 136), (138, 138, 137),
        (139, 139, 138), (141, 140, 140), (141, 141, 140), (142, 142, 141), (143, 143, 142), (145, 144, 144), (146, 145, 144), (146, 146, 145),
        (147, 147, 146), (149, 149, 148), (150, 149, 149), (151, 150, 149), (151, 151, 150), (153, 153, 152), (154, 154, 153), (155, 154, 154),
        (156, 155, 154), (157, 157, 156), (158, 158, 157), (159, 159, 158), (160, 160, 159), (162, 161, 160), (163, 162, 161), (163, 163, 162),
        (164, 164, 163), (166, 166, 165), (167, 166, 166), (168, 167, 167), (169, 168, 167), (170, 170, 169), (171, 171, 170), (172, 172, 171),
        (173, 173, 172), (175, 174, 174), (176, 175, 174), (177, 176, 175), (177, 177, 176), (179, 179, 178), (180, 180, 179), (181, 181, 180),
        (183, 183, 182), (184, 183, 183), (185, 184, 183), (186, 185, 184), (187, 187, 186), (188, 188, 187), (189, 189, 188), (190, 190, 189),
        (192, 192, 191), (193, 193, 192), (194, 194, 193), (195, 195, 194), (197, 197, 195), (198, 197, 196), (199, 198, 197), (200, 199, 198),
        (202, 201, 200), (203, 202, 201), (204, 203, 202), (204, 204, 203), (206, 206, 205), (207, 207, 206), (208, 208, 207), (209, 209, 208),
        (211, 211, 210), (212, 212, 211), (213, 213, 212), (214, 214, 213), (216, 216, 215), (217, 217, 216), (218, 218, 217), (219, 219, 218),
        (221, 221, 220), (222, 222, 221), (223, 223, 222), (224, 224, 223), (226, 226, 225), (227, 227, 226), (228, 228, 227), (230, 229, 228),
        (232, 231, 230), (233, 232, 231), (234, 233, 232), (235, 235, 233), (237, 237, 235), (238, 238, 236), (239, 239, 238), (240, 240, 239),
        (242, 242, 241), (243, 243, 242), (244, 244, 243), (248, 254, 105), (246, 253, 103), (245, 252, 100), (244, 252, 98), (241, 250, 93),
        (240, 249, 90), (239, 248, 87), (238, 247, 84), (236, 245, 78), (235, 243, 75), (235, 242, 72), (234, 241, 69), (234, 238, 64), (234, 237, 62),
        (234, 235, 61), (234, 234, 59), (234, 231, 56), (233, 230, 55), (233, 228, 54), (233, 227, 52), (233, 224, 50), (232, 223, 49), (232, 221, 48),
        (232, 220, 48), (232, 217, 46), (231, 216, 45), (231, 215, 44), (231, 213, 43), (230, 211, 42), (230, 209, 41), (230, 208, 40), (229, 207, 40),
        (229, 204, 38), (229, 203, 38), (228, 201, 37), (228, 200, 37), (227, 198, 35), (227, 196, 35), (227, 195, 34), (226, 194, 33), (226, 191, 32),
        (225, 190, 32), (225, 189, 31), (224, 187, 31), (224, 185, 30), (223, 184, 29), (223, 182, 29), (223, 181, 28), (222, 179, 27), (221, 177, 26),
        (221, 176, 26), (221, 175, 25)
    ];
}