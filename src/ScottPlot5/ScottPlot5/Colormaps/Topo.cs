﻿//This is a cmocean colormap
//All credit to Kristen Thyng
//This colormap is under the MIT License
//All cmocean colormaps are available at https://github.com/matplotlib/cmocean/tree/master/cmocean/rgb

namespace ScottPlot.Colormaps;

public class Topo : ByteColormapBase
{
    public override string Name => "Topo";

    public override (byte r, byte g, byte b)[] Rgbs => _rgbs;

    private static readonly (byte, byte, byte)[] _rgbs =
    [
        (40, 26, 44), (41, 28, 47), (43, 29, 50), (44, 31, 52), (45, 32, 55), (47, 34, 58), (48, 35, 61), (49, 37, 64), (50, 38, 67), (52, 40, 70),
        (53, 41, 73), (54, 42, 76), (55, 44, 79), (56, 45, 82), (57, 47, 85), (58, 48, 88), (59, 50, 92), (60, 51, 95), (61, 53, 98), (62, 54, 102),
        (63, 56, 105), (63, 57, 108), (64, 59, 112), (64, 60, 115), (65, 62, 118), (65, 64, 122), (65, 65, 125), (66, 67, 128), (65, 69, 131),
        (65, 71, 133), (65, 73, 136), (65, 75, 138), (64, 77, 140), (64, 79, 141), (63, 82, 143), (63, 84, 144), (62, 86, 145), (62, 88, 146),
        (62, 90, 146), (62, 92, 147), (62, 95, 147), (62, 97, 148), (62, 99, 148), (62, 101, 149), (62, 103, 149), (62, 105, 150), (62, 107, 150),
        (63, 109, 151), (63, 111, 151), (64, 113, 151), (64, 115, 152), (64, 117, 152), (65, 119, 153), (66, 121, 153), (66, 123, 153), (67, 125, 154),
        (67, 127, 154), (68, 129, 155), (68, 131, 155), (69, 133, 156), (70, 135, 156), (70, 137, 157), (71, 139, 157), (72, 141, 157), (72, 143, 158),
        (73, 145, 158), (74, 147, 159), (74, 149, 159), (75, 151, 160), (76, 153, 160), (77, 155, 161), (77, 157, 161), (78, 159, 161), (79, 161, 162),
        (80, 163, 162), (81, 165, 162), (81, 167, 163), (82, 169, 163), (83, 171, 163), (85, 173, 163), (86, 175, 164), (87, 177, 164), (88, 179, 164),
        (90, 182, 164), (91, 184, 164), (93, 186, 164), (95, 188, 164), (97, 190, 164), (99, 192, 164), (101, 194, 164), (103, 195, 164),
        (106, 197, 164), (109, 199, 163), (112, 201, 163), (115, 203, 163), (118, 205, 163), (122, 206, 163), (125, 208, 163), (129, 210, 163),
        (133, 211, 163), (137, 213, 163), (141, 215, 163), (146, 216, 164), (150, 218, 164), (154, 219, 165), (159, 221, 165), (163, 222, 166),
        (167, 224, 167), (172, 225, 168), (176, 226, 169), (181, 228, 170), (185, 229, 172), (189, 231, 173), (193, 232, 175), (198, 234, 176),
        (202, 235, 178), (206, 236, 179), (210, 238, 181), (215, 239, 183), (219, 241, 185), (223, 242, 187), (227, 244, 189), (231, 245, 191),
        (235, 247, 193), (239, 248, 196), (243, 250, 198), (247, 251, 200), (251, 253, 203), (13, 37, 20), (14, 39, 21), (15, 41, 21), (16, 42, 22),
        (17, 44, 23), (18, 46, 23), (19, 48, 24), (20, 50, 24), (21, 51, 25), (22, 53, 26), (23, 55, 26), (23, 57, 27), (24, 58, 27), (25, 60, 28),
        (26, 62, 28), (27, 64, 28), (27, 65, 29), (28, 67, 29), (29, 69, 29), (30, 71, 30), (31, 72, 30), (32, 74, 30), (33, 76, 30), (35, 78, 30),
        (37, 79, 30), (39, 81, 30), (42, 82, 30), (45, 83, 31), (48, 85, 32), (51, 86, 34), (54, 87, 35), (57, 88, 37), (60, 90, 38), (63, 91, 40),
        (65, 92, 42), (68, 93, 43), (71, 95, 45), (73, 96, 47), (76, 97, 48), (78, 98, 49), (81, 99, 51), (84, 101, 52), (86, 102, 53), (89, 103, 54),
        (92, 104, 55), (94, 106, 56), (97, 107, 57), (100, 108, 58), (102, 109, 58), (105, 111, 59), (107, 112, 60), (110, 113, 60), (113, 114, 61),
        (115, 116, 61), (118, 117, 62), (121, 118, 62), (123, 119, 62), (126, 121, 63), (129, 122, 63), (131, 123, 63), (134, 124, 63), (137, 126, 64),
        (140, 127, 64), (142, 128, 64), (145, 129, 64), (148, 131, 64), (150, 132, 64), (153, 133, 64), (156, 134, 64), (159, 136, 64), (161, 137, 64),
        (164, 138, 63), (167, 140, 63), (170, 141, 63), (173, 142, 63), (176, 143, 63), (179, 145, 63), (182, 146, 63), (185, 147, 62), (188, 148, 62),
        (191, 149, 63), (193, 151, 65), (195, 153, 69), (196, 155, 72), (197, 157, 76), (198, 159, 80), (199, 161, 83), (200, 163, 87), (202, 165, 90),
        (203, 167, 94), (204, 169, 97), (205, 171, 101), (206, 173, 104), (207, 176, 108), (208, 178, 111), (209, 180, 115), (210, 182, 118),
        (211, 184, 122), (212, 187, 125), (213, 189, 129), (214, 191, 132), (215, 193, 136), (216, 195, 139), (218, 198, 143), (219, 200, 146),
        (220, 202, 150), (221, 204, 153), (222, 207, 157), (223, 209, 160), (224, 211, 164), (226, 213, 167), (227, 216, 171), (228, 218, 174),
        (229, 220, 178), (230, 223, 182), (232, 225, 185), (233, 227, 189), (234, 230, 192), (236, 232, 196), (237, 234, 199), (238, 237, 203),
        (240, 239, 207), (241, 241, 210), (242, 244, 214), (244, 246, 217), (245, 249, 221), (247, 251, 225), (249, 253, 228)
    ];
}