﻿//This is a cmocean colormap
//All credit to Kristen Thyng
//This colormap is under the MIT License
//All cmocean colormaps are available at https://github.com/matplotlib/cmocean/tree/master/cmocean/rgb

namespace ScottPlot.Colormaps;

public class Ice : ByteColormapBase
{
    public override string Name => "Ice";

    public override (byte r, byte g, byte b)[] Rgbs => _rgbs;

    private static readonly (byte, byte, byte)[] _rgbs =
    [
        (4, 6, 19), (5, 6, 20), (5, 7, 21), (6, 8, 23), (7, 9, 24), (8, 10, 26), (9, 11, 27), (10, 12, 29), (11, 13, 30), (12, 13, 31), (13, 14, 33),
        (14, 15, 34), (15, 16, 36), (16, 17, 37), (17, 18, 39), (18, 19, 40), (19, 19, 42), (20, 20, 43), (21, 21, 44), (22, 22, 46), (23, 23, 47),
        (23, 24, 49), (24, 24, 50), (25, 25, 52), (26, 26, 53), (27, 27, 55), (28, 28, 56), (29, 28, 58), (30, 29, 59), (31, 30, 61), (31, 31, 62),
        (32, 31, 64), (33, 32, 65), (34, 33, 67), (35, 34, 68), (36, 34, 70), (37, 35, 71), (37, 36, 73), (38, 37, 74), (39, 37, 76), (40, 38, 78),
        (41, 39, 79), (41, 40, 81), (42, 40, 82), (43, 41, 84), (44, 42, 85), (44, 43, 87), (45, 43, 89), (46, 44, 90), (47, 45, 92), (47, 46, 94),
        (48, 47, 95), (49, 47, 97), (49, 48, 98), (50, 49, 100), (51, 50, 102), (51, 50, 103), (52, 51, 105), (53, 52, 107), (53, 53, 108),
        (54, 53, 110), (54, 54, 112), (55, 55, 113), (56, 56, 115), (56, 57, 117), (57, 57, 118), (57, 58, 120), (58, 59, 122), (58, 60, 123),
        (58, 61, 125), (59, 62, 127), (59, 62, 128), (60, 63, 130), (60, 64, 132), (60, 65, 133), (61, 66, 135), (61, 67, 137), (61, 68, 138),
        (62, 69, 140), (62, 70, 141), (62, 71, 143), (62, 72, 144), (62, 73, 146), (62, 73, 147), (63, 74, 149), (63, 75, 150), (63, 76, 151),
        (63, 78, 153), (63, 79, 154), (63, 80, 155), (63, 81, 157), (63, 82, 158), (63, 83, 159), (63, 84, 160), (63, 85, 161), (63, 86, 162),
        (63, 87, 163), (63, 88, 164), (63, 89, 165), (62, 90, 166), (62, 92, 167), (62, 93, 168), (62, 94, 169), (62, 95, 170), (62, 96, 171),
        (62, 97, 171), (62, 98, 172), (62, 99, 173), (62, 101, 173), (62, 102, 174), (62, 103, 175), (62, 104, 175), (62, 105, 176), (62, 106, 176),
        (63, 107, 177), (63, 108, 178), (63, 110, 178), (63, 111, 179), (63, 112, 179), (63, 113, 180), (64, 114, 180), (64, 115, 180), (64, 116, 181),
        (64, 117, 181), (65, 118, 182), (65, 120, 182), (66, 121, 183), (66, 122, 183), (66, 123, 183), (67, 124, 184), (67, 125, 184), (68, 126, 185),
        (68, 127, 185), (69, 128, 185), (69, 129, 186), (70, 130, 186), (70, 132, 187), (71, 133, 187), (71, 134, 187), (72, 135, 188), (73, 136, 188),
        (73, 137, 188), (74, 138, 189), (75, 139, 189), (75, 140, 189), (76, 141, 190), (77, 142, 190), (78, 143, 191), (78, 144, 191), (79, 145, 191),
        (80, 146, 192), (81, 148, 192), (81, 149, 192), (82, 150, 193), (83, 151, 193), (84, 152, 194), (85, 153, 194), (85, 154, 194), (86, 155, 195),
        (87, 156, 195), (88, 157, 195), (89, 158, 196), (90, 159, 196), (91, 160, 197), (92, 161, 197), (93, 162, 197), (94, 163, 198), (95, 164, 198),
        (95, 166, 199), (96, 167, 199), (97, 168, 199), (98, 169, 200), (99, 170, 200), (100, 171, 201), (101, 172, 201), (103, 173, 201),
        (104, 174, 202), (105, 175, 202), (106, 176, 203), (107, 177, 203), (108, 178, 203), (109, 179, 204), (110, 180, 204), (111, 181, 205),
        (113, 182, 205), (114, 184, 206), (115, 185, 206), (116, 186, 206), (117, 187, 207), (119, 188, 207), (120, 189, 208), (121, 190, 208),
        (123, 191, 208), (124, 192, 209), (125, 193, 209), (127, 194, 210), (128, 195, 210), (130, 196, 211), (131, 197, 211), (133, 198, 211),
        (134, 199, 212), (136, 200, 212), (137, 201, 213), (139, 202, 213), (140, 203, 214), (142, 204, 214), (144, 205, 215), (146, 206, 215),
        (147, 207, 216), (149, 208, 216), (151, 209, 217), (153, 210, 217), (154, 211, 218), (156, 212, 218), (158, 213, 219), (160, 214, 220),
        (162, 214, 220), (164, 215, 221), (166, 216, 222), (168, 217, 222), (169, 218, 223), (171, 219, 224), (173, 220, 224), (175, 221, 225),
        (177, 222, 226), (179, 223, 227), (181, 224, 227), (183, 225, 228), (185, 226, 229), (186, 227, 230), (188, 228, 231), (190, 229, 231),
        (192, 230, 232), (194, 230, 233), (196, 231, 234), (198, 232, 235), (200, 233, 236), (201, 234, 237), (203, 235, 238), (205, 236, 239),
        (207, 237, 239), (209, 238, 240), (211, 239, 241), (213, 240, 242), (214, 241, 243), (216, 242, 244), (218, 243, 245), (220, 244, 246),
        (222, 245, 247), (224, 246, 248), (225, 247, 249), (227, 249, 250), (229, 250, 251), (231, 251, 251), (232, 252, 252), (234, 253, 253)
    ];
}