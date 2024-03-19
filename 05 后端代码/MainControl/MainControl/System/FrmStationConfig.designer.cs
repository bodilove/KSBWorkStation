﻿namespace MainControl
{
    partial class FrmStationConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStationConfig));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_concel = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tab_baseInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvlocalStConfig = new System.Windows.Forms.DataGridView();
            this.ColStationNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvDevice = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tab_baseInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvlocalStConfig)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_save
            // 
            this.btn_save.Image = global::MainControl.Properties.Resources.save;
            this.btn_save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_save.Location = new System.Drawing.Point(208, 324);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(83, 32);
            this.btn_save.TabIndex = 27;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_concel
            // 
            this.btn_concel.Image = global::MainControl.Properties.Resources.delete;
            this.btn_concel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_concel.Location = new System.Drawing.Point(299, 324);
            this.btn_concel.Name = "btn_concel";
            this.btn_concel.Size = new System.Drawing.Size(83, 32);
            this.btn_concel.TabIndex = 28;
            this.btn_concel.Text = "关闭";
            this.btn_concel.UseVisualStyleBackColor = true;
            this.btn_concel.Click += new System.EventHandler(this.btn_concel_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "0.png");
            this.imageList1.Images.SetKeyName(1, "1.png");
            this.imageList1.Images.SetKeyName(2, "2.png");
            this.imageList1.Images.SetKeyName(3, "3.png");
            this.imageList1.Images.SetKeyName(4, "4.png");
            this.imageList1.Images.SetKeyName(5, "5.png");
            this.imageList1.Images.SetKeyName(6, "6.png");
            this.imageList1.Images.SetKeyName(7, "7.png");
            this.imageList1.Images.SetKeyName(8, "8.png");
            this.imageList1.Images.SetKeyName(9, "9.png");
            this.imageList1.Images.SetKeyName(10, "10.png");
            this.imageList1.Images.SetKeyName(11, "11.png");
            this.imageList1.Images.SetKeyName(12, "12.png");
            this.imageList1.Images.SetKeyName(13, "13.png");
            this.imageList1.Images.SetKeyName(14, "14.png");
            this.imageList1.Images.SetKeyName(15, "15.png");
            this.imageList1.Images.SetKeyName(16, "16.png");
            this.imageList1.Images.SetKeyName(17, "17.png");
            this.imageList1.Images.SetKeyName(18, "18.png");
            this.imageList1.Images.SetKeyName(19, "19.png");
            this.imageList1.Images.SetKeyName(20, "20.png");
            this.imageList1.Images.SetKeyName(21, "21.png");
            this.imageList1.Images.SetKeyName(22, "22.png");
            this.imageList1.Images.SetKeyName(23, "23.png");
            this.imageList1.Images.SetKeyName(24, "24.png");
            this.imageList1.Images.SetKeyName(25, "25.png");
            this.imageList1.Images.SetKeyName(26, "26.png");
            this.imageList1.Images.SetKeyName(27, "27.png");
            this.imageList1.Images.SetKeyName(28, "28.png");
            this.imageList1.Images.SetKeyName(29, "29.png");
            this.imageList1.Images.SetKeyName(30, "30.png");
            this.imageList1.Images.SetKeyName(31, "31.png");
            this.imageList1.Images.SetKeyName(32, "32.png");
            this.imageList1.Images.SetKeyName(33, "33.png");
            this.imageList1.Images.SetKeyName(34, "34.png");
            this.imageList1.Images.SetKeyName(35, "35.png");
            this.imageList1.Images.SetKeyName(36, "36.png");
            this.imageList1.Images.SetKeyName(37, "37.png");
            this.imageList1.Images.SetKeyName(38, "38.png");
            this.imageList1.Images.SetKeyName(39, "39.png");
            this.imageList1.Images.SetKeyName(40, "40.png");
            this.imageList1.Images.SetKeyName(41, "41.png");
            this.imageList1.Images.SetKeyName(42, "42.png");
            this.imageList1.Images.SetKeyName(43, "43.png");
            this.imageList1.Images.SetKeyName(44, "44.png");
            this.imageList1.Images.SetKeyName(45, "45.png");
            this.imageList1.Images.SetKeyName(46, "46.png");
            this.imageList1.Images.SetKeyName(47, "47.png");
            this.imageList1.Images.SetKeyName(48, "48.png");
            this.imageList1.Images.SetKeyName(49, "49.png");
            this.imageList1.Images.SetKeyName(50, "50.png");
            this.imageList1.Images.SetKeyName(51, "51.png");
            this.imageList1.Images.SetKeyName(52, "52.png");
            this.imageList1.Images.SetKeyName(53, "53.png");
            this.imageList1.Images.SetKeyName(54, "54.png");
            this.imageList1.Images.SetKeyName(55, "55.png");
            this.imageList1.Images.SetKeyName(56, "56.png");
            this.imageList1.Images.SetKeyName(57, "57.png");
            this.imageList1.Images.SetKeyName(58, "58.png");
            this.imageList1.Images.SetKeyName(59, "59.png");
            this.imageList1.Images.SetKeyName(60, "60.png");
            this.imageList1.Images.SetKeyName(61, "61.png");
            this.imageList1.Images.SetKeyName(62, "62.png");
            this.imageList1.Images.SetKeyName(63, "63.png");
            this.imageList1.Images.SetKeyName(64, "64.png");
            this.imageList1.Images.SetKeyName(65, "65.png");
            this.imageList1.Images.SetKeyName(66, "66.png");
            this.imageList1.Images.SetKeyName(67, "67.png");
            this.imageList1.Images.SetKeyName(68, "68.png");
            this.imageList1.Images.SetKeyName(69, "69.png");
            this.imageList1.Images.SetKeyName(70, "70.png");
            this.imageList1.Images.SetKeyName(71, "71.png");
            this.imageList1.Images.SetKeyName(72, "72.png");
            this.imageList1.Images.SetKeyName(73, "73.png");
            this.imageList1.Images.SetKeyName(74, "74.png");
            this.imageList1.Images.SetKeyName(75, "75.png");
            this.imageList1.Images.SetKeyName(76, "76.png");
            this.imageList1.Images.SetKeyName(77, "77.png");
            this.imageList1.Images.SetKeyName(78, "78.png");
            this.imageList1.Images.SetKeyName(79, "79.png");
            this.imageList1.Images.SetKeyName(80, "80.png");
            this.imageList1.Images.SetKeyName(81, "81.png");
            this.imageList1.Images.SetKeyName(82, "82.png");
            this.imageList1.Images.SetKeyName(83, "83.png");
            this.imageList1.Images.SetKeyName(84, "84.png");
            this.imageList1.Images.SetKeyName(85, "85.png");
            this.imageList1.Images.SetKeyName(86, "86.png");
            this.imageList1.Images.SetKeyName(87, "87.png");
            this.imageList1.Images.SetKeyName(88, "88.png");
            this.imageList1.Images.SetKeyName(89, "89.png");
            this.imageList1.Images.SetKeyName(90, "90.png");
            this.imageList1.Images.SetKeyName(91, "91.png");
            this.imageList1.Images.SetKeyName(92, "92.png");
            this.imageList1.Images.SetKeyName(93, "93.png");
            this.imageList1.Images.SetKeyName(94, "94.png");
            this.imageList1.Images.SetKeyName(95, "95.png");
            this.imageList1.Images.SetKeyName(96, "96.png");
            this.imageList1.Images.SetKeyName(97, "97.png");
            this.imageList1.Images.SetKeyName(98, "98.png");
            this.imageList1.Images.SetKeyName(99, "99.png");
            this.imageList1.Images.SetKeyName(100, "100.png");
            this.imageList1.Images.SetKeyName(101, "101.png");
            this.imageList1.Images.SetKeyName(102, "102.png");
            this.imageList1.Images.SetKeyName(103, "103.png");
            this.imageList1.Images.SetKeyName(104, "104.png");
            this.imageList1.Images.SetKeyName(105, "105.png");
            this.imageList1.Images.SetKeyName(106, "106.png");
            this.imageList1.Images.SetKeyName(107, "107.png");
            this.imageList1.Images.SetKeyName(108, "108.png");
            this.imageList1.Images.SetKeyName(109, "109.png");
            this.imageList1.Images.SetKeyName(110, "110.png");
            this.imageList1.Images.SetKeyName(111, "111.png");
            this.imageList1.Images.SetKeyName(112, "112.png");
            this.imageList1.Images.SetKeyName(113, "113.png");
            this.imageList1.Images.SetKeyName(114, "114.png");
            this.imageList1.Images.SetKeyName(115, "115.png");
            this.imageList1.Images.SetKeyName(116, "116.png");
            this.imageList1.Images.SetKeyName(117, "117.png");
            this.imageList1.Images.SetKeyName(118, "118.png");
            this.imageList1.Images.SetKeyName(119, "119.png");
            this.imageList1.Images.SetKeyName(120, "120.png");
            this.imageList1.Images.SetKeyName(121, "121.png");
            this.imageList1.Images.SetKeyName(122, "122.png");
            this.imageList1.Images.SetKeyName(123, "123.png");
            this.imageList1.Images.SetKeyName(124, "124.png");
            this.imageList1.Images.SetKeyName(125, "125.png");
            this.imageList1.Images.SetKeyName(126, "126.png");
            this.imageList1.Images.SetKeyName(127, "127.png");
            this.imageList1.Images.SetKeyName(128, "128.png");
            this.imageList1.Images.SetKeyName(129, "129.png");
            this.imageList1.Images.SetKeyName(130, "130.png");
            this.imageList1.Images.SetKeyName(131, "131.png");
            this.imageList1.Images.SetKeyName(132, "132.png");
            this.imageList1.Images.SetKeyName(133, "133.png");
            this.imageList1.Images.SetKeyName(134, "134.png");
            this.imageList1.Images.SetKeyName(135, "135.png");
            this.imageList1.Images.SetKeyName(136, "136.png");
            this.imageList1.Images.SetKeyName(137, "137.png");
            this.imageList1.Images.SetKeyName(138, "138.png");
            this.imageList1.Images.SetKeyName(139, "139.png");
            this.imageList1.Images.SetKeyName(140, "140.png");
            this.imageList1.Images.SetKeyName(141, "141.png");
            this.imageList1.Images.SetKeyName(142, "142.png");
            this.imageList1.Images.SetKeyName(143, "143.png");
            this.imageList1.Images.SetKeyName(144, "144.png");
            this.imageList1.Images.SetKeyName(145, "145.png");
            this.imageList1.Images.SetKeyName(146, "146.png");
            this.imageList1.Images.SetKeyName(147, "147.png");
            this.imageList1.Images.SetKeyName(148, "148.png");
            this.imageList1.Images.SetKeyName(149, "149.png");
            this.imageList1.Images.SetKeyName(150, "150.png");
            this.imageList1.Images.SetKeyName(151, "151.png");
            this.imageList1.Images.SetKeyName(152, "152.png");
            this.imageList1.Images.SetKeyName(153, "153.png");
            this.imageList1.Images.SetKeyName(154, "154.png");
            this.imageList1.Images.SetKeyName(155, "155.png");
            this.imageList1.Images.SetKeyName(156, "156.png");
            this.imageList1.Images.SetKeyName(157, "157.png");
            this.imageList1.Images.SetKeyName(158, "158.png");
            this.imageList1.Images.SetKeyName(159, "159.png");
            this.imageList1.Images.SetKeyName(160, "160.png");
            this.imageList1.Images.SetKeyName(161, "161.png");
            this.imageList1.Images.SetKeyName(162, "162.png");
            this.imageList1.Images.SetKeyName(163, "163.png");
            this.imageList1.Images.SetKeyName(164, "164.png");
            this.imageList1.Images.SetKeyName(165, "165.png");
            this.imageList1.Images.SetKeyName(166, "166.png");
            this.imageList1.Images.SetKeyName(167, "167.png");
            this.imageList1.Images.SetKeyName(168, "168.png");
            this.imageList1.Images.SetKeyName(169, "169.png");
            this.imageList1.Images.SetKeyName(170, "170.png");
            this.imageList1.Images.SetKeyName(171, "171.png");
            this.imageList1.Images.SetKeyName(172, "172.png");
            this.imageList1.Images.SetKeyName(173, "173.png");
            this.imageList1.Images.SetKeyName(174, "174.png");
            this.imageList1.Images.SetKeyName(175, "175.png");
            this.imageList1.Images.SetKeyName(176, "176.png");
            this.imageList1.Images.SetKeyName(177, "177.png");
            this.imageList1.Images.SetKeyName(178, "178.png");
            this.imageList1.Images.SetKeyName(179, "179.png");
            this.imageList1.Images.SetKeyName(180, "180.png");
            this.imageList1.Images.SetKeyName(181, "181.png");
            this.imageList1.Images.SetKeyName(182, "182.png");
            this.imageList1.Images.SetKeyName(183, "183.png");
            this.imageList1.Images.SetKeyName(184, "184.png");
            this.imageList1.Images.SetKeyName(185, "185.png");
            this.imageList1.Images.SetKeyName(186, "186.png");
            this.imageList1.Images.SetKeyName(187, "187.png");
            this.imageList1.Images.SetKeyName(188, "188.png");
            this.imageList1.Images.SetKeyName(189, "189.png");
            this.imageList1.Images.SetKeyName(190, "190.png");
            this.imageList1.Images.SetKeyName(191, "191.png");
            this.imageList1.Images.SetKeyName(192, "192.png");
            this.imageList1.Images.SetKeyName(193, "193.png");
            this.imageList1.Images.SetKeyName(194, "194.png");
            this.imageList1.Images.SetKeyName(195, "195.png");
            this.imageList1.Images.SetKeyName(196, "196.png");
            this.imageList1.Images.SetKeyName(197, "197.png");
            this.imageList1.Images.SetKeyName(198, "198.png");
            this.imageList1.Images.SetKeyName(199, "199.png");
            this.imageList1.Images.SetKeyName(200, "200.png");
            this.imageList1.Images.SetKeyName(201, "201.png");
            this.imageList1.Images.SetKeyName(202, "202.png");
            this.imageList1.Images.SetKeyName(203, "203.png");
            this.imageList1.Images.SetKeyName(204, "204.png");
            this.imageList1.Images.SetKeyName(205, "205.png");
            this.imageList1.Images.SetKeyName(206, "206.png");
            this.imageList1.Images.SetKeyName(207, "207.png");
            this.imageList1.Images.SetKeyName(208, "208.png");
            this.imageList1.Images.SetKeyName(209, "209.png");
            this.imageList1.Images.SetKeyName(210, "210.png");
            this.imageList1.Images.SetKeyName(211, "211.png");
            this.imageList1.Images.SetKeyName(212, "212.png");
            this.imageList1.Images.SetKeyName(213, "213.png");
            this.imageList1.Images.SetKeyName(214, "214.png");
            this.imageList1.Images.SetKeyName(215, "215.png");
            this.imageList1.Images.SetKeyName(216, "216.png");
            this.imageList1.Images.SetKeyName(217, "217.png");
            this.imageList1.Images.SetKeyName(218, "218.png");
            this.imageList1.Images.SetKeyName(219, "219.png");
            this.imageList1.Images.SetKeyName(220, "220.png");
            this.imageList1.Images.SetKeyName(221, "221.png");
            this.imageList1.Images.SetKeyName(222, "222.png");
            this.imageList1.Images.SetKeyName(223, "223.png");
            this.imageList1.Images.SetKeyName(224, "224.png");
            this.imageList1.Images.SetKeyName(225, "225.png");
            this.imageList1.Images.SetKeyName(226, "226.png");
            this.imageList1.Images.SetKeyName(227, "227.png");
            this.imageList1.Images.SetKeyName(228, "228.png");
            this.imageList1.Images.SetKeyName(229, "229.png");
            this.imageList1.Images.SetKeyName(230, "230.png");
            this.imageList1.Images.SetKeyName(231, "231.png");
            this.imageList1.Images.SetKeyName(232, "232.png");
            this.imageList1.Images.SetKeyName(233, "233.png");
            this.imageList1.Images.SetKeyName(234, "234.png");
            this.imageList1.Images.SetKeyName(235, "235.png");
            this.imageList1.Images.SetKeyName(236, "236.png");
            this.imageList1.Images.SetKeyName(237, "237.png");
            this.imageList1.Images.SetKeyName(238, "238.png");
            this.imageList1.Images.SetKeyName(239, "239.png");
            this.imageList1.Images.SetKeyName(240, "240.png");
            this.imageList1.Images.SetKeyName(241, "241.png");
            this.imageList1.Images.SetKeyName(242, "242.png");
            this.imageList1.Images.SetKeyName(243, "243.png");
            this.imageList1.Images.SetKeyName(244, "244.png");
            this.imageList1.Images.SetKeyName(245, "245.png");
            this.imageList1.Images.SetKeyName(246, "246.png");
            this.imageList1.Images.SetKeyName(247, "247.png");
            this.imageList1.Images.SetKeyName(248, "248.png");
            this.imageList1.Images.SetKeyName(249, "249.png");
            this.imageList1.Images.SetKeyName(250, "250.png");
            this.imageList1.Images.SetKeyName(251, "251.png");
            this.imageList1.Images.SetKeyName(252, "252.png");
            this.imageList1.Images.SetKeyName(253, "253.png");
            this.imageList1.Images.SetKeyName(254, "254.png");
            this.imageList1.Images.SetKeyName(255, "255.png");
            this.imageList1.Images.SetKeyName(256, "256.png");
            this.imageList1.Images.SetKeyName(257, "257.png");
            this.imageList1.Images.SetKeyName(258, "258.png");
            this.imageList1.Images.SetKeyName(259, "259.png");
            this.imageList1.Images.SetKeyName(260, "260.png");
            this.imageList1.Images.SetKeyName(261, "261.png");
            this.imageList1.Images.SetKeyName(262, "262.png");
            this.imageList1.Images.SetKeyName(263, "263.png");
            this.imageList1.Images.SetKeyName(264, "264.png");
            this.imageList1.Images.SetKeyName(265, "265.png");
            this.imageList1.Images.SetKeyName(266, "266.png");
            this.imageList1.Images.SetKeyName(267, "267.png");
            this.imageList1.Images.SetKeyName(268, "268.png");
            this.imageList1.Images.SetKeyName(269, "269.png");
            this.imageList1.Images.SetKeyName(270, "270.png");
            this.imageList1.Images.SetKeyName(271, "271.png");
            this.imageList1.Images.SetKeyName(272, "272.png");
            this.imageList1.Images.SetKeyName(273, "273.png");
            this.imageList1.Images.SetKeyName(274, "274.png");
            this.imageList1.Images.SetKeyName(275, "275.png");
            this.imageList1.Images.SetKeyName(276, "276.png");
            this.imageList1.Images.SetKeyName(277, "277.png");
            this.imageList1.Images.SetKeyName(278, "278.png");
            this.imageList1.Images.SetKeyName(279, "279.png");
            this.imageList1.Images.SetKeyName(280, "280.png");
            this.imageList1.Images.SetKeyName(281, "281.png");
            this.imageList1.Images.SetKeyName(282, "282.png");
            this.imageList1.Images.SetKeyName(283, "283.png");
            this.imageList1.Images.SetKeyName(284, "284.png");
            this.imageList1.Images.SetKeyName(285, "285.png");
            this.imageList1.Images.SetKeyName(286, "286.png");
            this.imageList1.Images.SetKeyName(287, "287.png");
            this.imageList1.Images.SetKeyName(288, "288.png");
            this.imageList1.Images.SetKeyName(289, "289.png");
            this.imageList1.Images.SetKeyName(290, "290.png");
            this.imageList1.Images.SetKeyName(291, "291.png");
            this.imageList1.Images.SetKeyName(292, "292.png");
            this.imageList1.Images.SetKeyName(293, "293.png");
            this.imageList1.Images.SetKeyName(294, "294.png");
            this.imageList1.Images.SetKeyName(295, "295.png");
            this.imageList1.Images.SetKeyName(296, "296.png");
            this.imageList1.Images.SetKeyName(297, "297.png");
            this.imageList1.Images.SetKeyName(298, "298.png");
            this.imageList1.Images.SetKeyName(299, "299.png");
            this.imageList1.Images.SetKeyName(300, "300.png");
            this.imageList1.Images.SetKeyName(301, "301.png");
            this.imageList1.Images.SetKeyName(302, "302.png");
            this.imageList1.Images.SetKeyName(303, "303.png");
            this.imageList1.Images.SetKeyName(304, "304.png");
            this.imageList1.Images.SetKeyName(305, "305.png");
            this.imageList1.Images.SetKeyName(306, "306.png");
            this.imageList1.Images.SetKeyName(307, "307.png");
            this.imageList1.Images.SetKeyName(308, "308.png");
            this.imageList1.Images.SetKeyName(309, "309.png");
            this.imageList1.Images.SetKeyName(310, "310.png");
            this.imageList1.Images.SetKeyName(311, "311.png");
            this.imageList1.Images.SetKeyName(312, "312.png");
            this.imageList1.Images.SetKeyName(313, "313.png");
            this.imageList1.Images.SetKeyName(314, "314.png");
            this.imageList1.Images.SetKeyName(315, "315.png");
            this.imageList1.Images.SetKeyName(316, "316.png");
            this.imageList1.Images.SetKeyName(317, "317.png");
            this.imageList1.Images.SetKeyName(318, "318.png");
            this.imageList1.Images.SetKeyName(319, "319.png");
            this.imageList1.Images.SetKeyName(320, "320.png");
            this.imageList1.Images.SetKeyName(321, "321.png");
            this.imageList1.Images.SetKeyName(322, "322.png");
            this.imageList1.Images.SetKeyName(323, "323.png");
            this.imageList1.Images.SetKeyName(324, "324.png");
            this.imageList1.Images.SetKeyName(325, "325.png");
            this.imageList1.Images.SetKeyName(326, "326.png");
            this.imageList1.Images.SetKeyName(327, "327.png");
            this.imageList1.Images.SetKeyName(328, "328.png");
            this.imageList1.Images.SetKeyName(329, "329.png");
            this.imageList1.Images.SetKeyName(330, "330.png");
            this.imageList1.Images.SetKeyName(331, "331.png");
            this.imageList1.Images.SetKeyName(332, "332.png");
            this.imageList1.Images.SetKeyName(333, "333.png");
            this.imageList1.Images.SetKeyName(334, "334.png");
            this.imageList1.Images.SetKeyName(335, "335.png");
            this.imageList1.Images.SetKeyName(336, "336.png");
            this.imageList1.Images.SetKeyName(337, "337.png");
            this.imageList1.Images.SetKeyName(338, "338.png");
            this.imageList1.Images.SetKeyName(339, "339.png");
            this.imageList1.Images.SetKeyName(340, "340.png");
            this.imageList1.Images.SetKeyName(341, "341.png");
            this.imageList1.Images.SetKeyName(342, "342.png");
            this.imageList1.Images.SetKeyName(343, "343.png");
            this.imageList1.Images.SetKeyName(344, "344.png");
            this.imageList1.Images.SetKeyName(345, "345.png");
            this.imageList1.Images.SetKeyName(346, "346.png");
            this.imageList1.Images.SetKeyName(347, "347.png");
            this.imageList1.Images.SetKeyName(348, "348.png");
            this.imageList1.Images.SetKeyName(349, "349.png");
            this.imageList1.Images.SetKeyName(350, "350.png");
            this.imageList1.Images.SetKeyName(351, "351.png");
            this.imageList1.Images.SetKeyName(352, "352.png");
            this.imageList1.Images.SetKeyName(353, "353.png");
            this.imageList1.Images.SetKeyName(354, "354.png");
            this.imageList1.Images.SetKeyName(355, "355.png");
            this.imageList1.Images.SetKeyName(356, "356.png");
            this.imageList1.Images.SetKeyName(357, "357.png");
            this.imageList1.Images.SetKeyName(358, "358.png");
            this.imageList1.Images.SetKeyName(359, "359.png");
            this.imageList1.Images.SetKeyName(360, "360.png");
            this.imageList1.Images.SetKeyName(361, "361.png");
            this.imageList1.Images.SetKeyName(362, "362.png");
            this.imageList1.Images.SetKeyName(363, "363.png");
            this.imageList1.Images.SetKeyName(364, "364.png");
            this.imageList1.Images.SetKeyName(365, "365.png");
            this.imageList1.Images.SetKeyName(366, "366.png");
            this.imageList1.Images.SetKeyName(367, "367.png");
            this.imageList1.Images.SetKeyName(368, "368.png");
            this.imageList1.Images.SetKeyName(369, "369.png");
            this.imageList1.Images.SetKeyName(370, "370.png");
            this.imageList1.Images.SetKeyName(371, "371.png");
            this.imageList1.Images.SetKeyName(372, "372.png");
            this.imageList1.Images.SetKeyName(373, "373.png");
            this.imageList1.Images.SetKeyName(374, "374.png");
            this.imageList1.Images.SetKeyName(375, "375.png");
            this.imageList1.Images.SetKeyName(376, "376.png");
            this.imageList1.Images.SetKeyName(377, "377.png");
            this.imageList1.Images.SetKeyName(378, "378.png");
            this.imageList1.Images.SetKeyName(379, "379.png");
            this.imageList1.Images.SetKeyName(380, "380.png");
            this.imageList1.Images.SetKeyName(381, "381.png");
            this.imageList1.Images.SetKeyName(382, "382.png");
            this.imageList1.Images.SetKeyName(383, "383.png");
            this.imageList1.Images.SetKeyName(384, "384.png");
            this.imageList1.Images.SetKeyName(385, "385.png");
            this.imageList1.Images.SetKeyName(386, "386.png");
            this.imageList1.Images.SetKeyName(387, "387.png");
            this.imageList1.Images.SetKeyName(388, "388.png");
            this.imageList1.Images.SetKeyName(389, "389.png");
            this.imageList1.Images.SetKeyName(390, "390.png");
            this.imageList1.Images.SetKeyName(391, "391.png");
            this.imageList1.Images.SetKeyName(392, "392.png");
            this.imageList1.Images.SetKeyName(393, "393.png");
            this.imageList1.Images.SetKeyName(394, "394.png");
            this.imageList1.Images.SetKeyName(395, "395.png");
            this.imageList1.Images.SetKeyName(396, "396.png");
            this.imageList1.Images.SetKeyName(397, "397.png");
            this.imageList1.Images.SetKeyName(398, "398.png");
            this.imageList1.Images.SetKeyName(399, "399.png");
            this.imageList1.Images.SetKeyName(400, "400.png");
            this.imageList1.Images.SetKeyName(401, "401.png");
            this.imageList1.Images.SetKeyName(402, "402.png");
            this.imageList1.Images.SetKeyName(403, "403.png");
            this.imageList1.Images.SetKeyName(404, "404.png");
            this.imageList1.Images.SetKeyName(405, "405.png");
            this.imageList1.Images.SetKeyName(406, "406.png");
            this.imageList1.Images.SetKeyName(407, "407.png");
            this.imageList1.Images.SetKeyName(408, "408.png");
            this.imageList1.Images.SetKeyName(409, "409.png");
            this.imageList1.Images.SetKeyName(410, "410.png");
            this.imageList1.Images.SetKeyName(411, "411.png");
            this.imageList1.Images.SetKeyName(412, "412.png");
            this.imageList1.Images.SetKeyName(413, "413.png");
            this.imageList1.Images.SetKeyName(414, "414.png");
            this.imageList1.Images.SetKeyName(415, "415.png");
            this.imageList1.Images.SetKeyName(416, "416.png");
            this.imageList1.Images.SetKeyName(417, "417.png");
            this.imageList1.Images.SetKeyName(418, "418.png");
            this.imageList1.Images.SetKeyName(419, "419.png");
            this.imageList1.Images.SetKeyName(420, "420.png");
            this.imageList1.Images.SetKeyName(421, "421.png");
            this.imageList1.Images.SetKeyName(422, "422.png");
            this.imageList1.Images.SetKeyName(423, "423.png");
            this.imageList1.Images.SetKeyName(424, "424.png");
            this.imageList1.Images.SetKeyName(425, "425.png");
            this.imageList1.Images.SetKeyName(426, "426.png");
            this.imageList1.Images.SetKeyName(427, "427.png");
            this.imageList1.Images.SetKeyName(428, "428.png");
            this.imageList1.Images.SetKeyName(429, "429.png");
            this.imageList1.Images.SetKeyName(430, "430.png");
            this.imageList1.Images.SetKeyName(431, "431.png");
            this.imageList1.Images.SetKeyName(432, "432.png");
            this.imageList1.Images.SetKeyName(433, "433.png");
            this.imageList1.Images.SetKeyName(434, "434.png");
            this.imageList1.Images.SetKeyName(435, "435.png");
            this.imageList1.Images.SetKeyName(436, "436.png");
            this.imageList1.Images.SetKeyName(437, "437.png");
            this.imageList1.Images.SetKeyName(438, "438.png");
            this.imageList1.Images.SetKeyName(439, "439.png");
            this.imageList1.Images.SetKeyName(440, "440.png");
            this.imageList1.Images.SetKeyName(441, "441.png");
            this.imageList1.Images.SetKeyName(442, "442.png");
            this.imageList1.Images.SetKeyName(443, "443.png");
            this.imageList1.Images.SetKeyName(444, "444.png");
            this.imageList1.Images.SetKeyName(445, "445.png");
            this.imageList1.Images.SetKeyName(446, "446.png");
            this.imageList1.Images.SetKeyName(447, "447.png");
            this.imageList1.Images.SetKeyName(448, "448.png");
            this.imageList1.Images.SetKeyName(449, "449.png");
            this.imageList1.Images.SetKeyName(450, "450.png");
            this.imageList1.Images.SetKeyName(451, "451.png");
            this.imageList1.Images.SetKeyName(452, "452.png");
            this.imageList1.Images.SetKeyName(453, "453.png");
            this.imageList1.Images.SetKeyName(454, "454.png");
            this.imageList1.Images.SetKeyName(455, "455.png");
            this.imageList1.Images.SetKeyName(456, "456.png");
            this.imageList1.Images.SetKeyName(457, "457.png");
            this.imageList1.Images.SetKeyName(458, "458.png");
            this.imageList1.Images.SetKeyName(459, "459.png");
            this.imageList1.Images.SetKeyName(460, "460.png");
            this.imageList1.Images.SetKeyName(461, "461.png");
            this.imageList1.Images.SetKeyName(462, "462.png");
            this.imageList1.Images.SetKeyName(463, "463.png");
            this.imageList1.Images.SetKeyName(464, "464.png");
            this.imageList1.Images.SetKeyName(465, "465.png");
            this.imageList1.Images.SetKeyName(466, "466.png");
            this.imageList1.Images.SetKeyName(467, "467.png");
            this.imageList1.Images.SetKeyName(468, "468.png");
            this.imageList1.Images.SetKeyName(469, "469.png");
            this.imageList1.Images.SetKeyName(470, "470.png");
            this.imageList1.Images.SetKeyName(471, "471.png");
            this.imageList1.Images.SetKeyName(472, "472.png");
            this.imageList1.Images.SetKeyName(473, "473.png");
            this.imageList1.Images.SetKeyName(474, "474.png");
            this.imageList1.Images.SetKeyName(475, "475.png");
            this.imageList1.Images.SetKeyName(476, "476.png");
            this.imageList1.Images.SetKeyName(477, "477.png");
            this.imageList1.Images.SetKeyName(478, "478.png");
            this.imageList1.Images.SetKeyName(479, "479.png");
            this.imageList1.Images.SetKeyName(480, "480.png");
            this.imageList1.Images.SetKeyName(481, "481.png");
            this.imageList1.Images.SetKeyName(482, "482.png");
            this.imageList1.Images.SetKeyName(483, "483.png");
            this.imageList1.Images.SetKeyName(484, "484.png");
            this.imageList1.Images.SetKeyName(485, "485.png");
            this.imageList1.Images.SetKeyName(486, "486.png");
            this.imageList1.Images.SetKeyName(487, "487.png");
            this.imageList1.Images.SetKeyName(488, "488.png");
            this.imageList1.Images.SetKeyName(489, "489.png");
            this.imageList1.Images.SetKeyName(490, "490.png");
            this.imageList1.Images.SetKeyName(491, "491.png");
            this.imageList1.Images.SetKeyName(492, "492.png");
            this.imageList1.Images.SetKeyName(493, "493.png");
            this.imageList1.Images.SetKeyName(494, "494.png");
            this.imageList1.Images.SetKeyName(495, "495.png");
            this.imageList1.Images.SetKeyName(496, "496.png");
            this.imageList1.Images.SetKeyName(497, "497.png");
            this.imageList1.Images.SetKeyName(498, "498.png");
            this.imageList1.Images.SetKeyName(499, "499.png");
            this.imageList1.Images.SetKeyName(500, "500.png");
            this.imageList1.Images.SetKeyName(501, "501.png");
            this.imageList1.Images.SetKeyName(502, "502.png");
            this.imageList1.Images.SetKeyName(503, "503.png");
            this.imageList1.Images.SetKeyName(504, "504.png");
            this.imageList1.Images.SetKeyName(505, "505.png");
            this.imageList1.Images.SetKeyName(506, "506.png");
            this.imageList1.Images.SetKeyName(507, "507.png");
            this.imageList1.Images.SetKeyName(508, "508.png");
            this.imageList1.Images.SetKeyName(509, "509.png");
            this.imageList1.Images.SetKeyName(510, "510.png");
            this.imageList1.Images.SetKeyName(511, "511.png");
            this.imageList1.Images.SetKeyName(512, "512.png");
            this.imageList1.Images.SetKeyName(513, "513.png");
            this.imageList1.Images.SetKeyName(514, "514.png");
            this.imageList1.Images.SetKeyName(515, "515.png");
            this.imageList1.Images.SetKeyName(516, "516.png");
            this.imageList1.Images.SetKeyName(517, "517.png");
            this.imageList1.Images.SetKeyName(518, "518.png");
            this.imageList1.Images.SetKeyName(519, "519.png");
            this.imageList1.Images.SetKeyName(520, "520.png");
            this.imageList1.Images.SetKeyName(521, "521.png");
            this.imageList1.Images.SetKeyName(522, "522.png");
            this.imageList1.Images.SetKeyName(523, "523.png");
            this.imageList1.Images.SetKeyName(524, "524.png");
            this.imageList1.Images.SetKeyName(525, "525.png");
            this.imageList1.Images.SetKeyName(526, "526.png");
            this.imageList1.Images.SetKeyName(527, "527.png");
            this.imageList1.Images.SetKeyName(528, "528.png");
            this.imageList1.Images.SetKeyName(529, "529.png");
            this.imageList1.Images.SetKeyName(530, "530.png");
            this.imageList1.Images.SetKeyName(531, "531.png");
            this.imageList1.Images.SetKeyName(532, "532.png");
            this.imageList1.Images.SetKeyName(533, "533.png");
            this.imageList1.Images.SetKeyName(534, "534.png");
            this.imageList1.Images.SetKeyName(535, "535.png");
            this.imageList1.Images.SetKeyName(536, "536.png");
            this.imageList1.Images.SetKeyName(537, "537.png");
            this.imageList1.Images.SetKeyName(538, "538.png");
            this.imageList1.Images.SetKeyName(539, "539.png");
            this.imageList1.Images.SetKeyName(540, "540.png");
            this.imageList1.Images.SetKeyName(541, "541.png");
            this.imageList1.Images.SetKeyName(542, "542.png");
            this.imageList1.Images.SetKeyName(543, "543.png");
            this.imageList1.Images.SetKeyName(544, "544.png");
            this.imageList1.Images.SetKeyName(545, "545.png");
            this.imageList1.Images.SetKeyName(546, "546.png");
            this.imageList1.Images.SetKeyName(547, "547.png");
            this.imageList1.Images.SetKeyName(548, "548.png");
            this.imageList1.Images.SetKeyName(549, "549.png");
            this.imageList1.Images.SetKeyName(550, "550.png");
            this.imageList1.Images.SetKeyName(551, "551.png");
            this.imageList1.Images.SetKeyName(552, "552.png");
            this.imageList1.Images.SetKeyName(553, "553.png");
            this.imageList1.Images.SetKeyName(554, "554.png");
            this.imageList1.Images.SetKeyName(555, "555.png");
            this.imageList1.Images.SetKeyName(556, "556.png");
            this.imageList1.Images.SetKeyName(557, "557.png");
            this.imageList1.Images.SetKeyName(558, "558.png");
            this.imageList1.Images.SetKeyName(559, "559.png");
            this.imageList1.Images.SetKeyName(560, "560.png");
            this.imageList1.Images.SetKeyName(561, "561.png");
            this.imageList1.Images.SetKeyName(562, "562.png");
            this.imageList1.Images.SetKeyName(563, "563.png");
            this.imageList1.Images.SetKeyName(564, "564.png");
            this.imageList1.Images.SetKeyName(565, "565.png");
            this.imageList1.Images.SetKeyName(566, "566.png");
            this.imageList1.Images.SetKeyName(567, "567.png");
            this.imageList1.Images.SetKeyName(568, "568.png");
            this.imageList1.Images.SetKeyName(569, "569.png");
            this.imageList1.Images.SetKeyName(570, "570.png");
            this.imageList1.Images.SetKeyName(571, "571.png");
            this.imageList1.Images.SetKeyName(572, "572.png");
            this.imageList1.Images.SetKeyName(573, "573.png");
            this.imageList1.Images.SetKeyName(574, "574.png");
            this.imageList1.Images.SetKeyName(575, "575.png");
            this.imageList1.Images.SetKeyName(576, "576.png");
            this.imageList1.Images.SetKeyName(577, "577.png");
            this.imageList1.Images.SetKeyName(578, "578.png");
            this.imageList1.Images.SetKeyName(579, "579.png");
            this.imageList1.Images.SetKeyName(580, "580.png");
            this.imageList1.Images.SetKeyName(581, "581.png");
            this.imageList1.Images.SetKeyName(582, "582.png");
            this.imageList1.Images.SetKeyName(583, "583.png");
            this.imageList1.Images.SetKeyName(584, "584.png");
            this.imageList1.Images.SetKeyName(585, "585.png");
            this.imageList1.Images.SetKeyName(586, "586.png");
            this.imageList1.Images.SetKeyName(587, "587.png");
            this.imageList1.Images.SetKeyName(588, "588.png");
            this.imageList1.Images.SetKeyName(589, "589.png");
            this.imageList1.Images.SetKeyName(590, "590.png");
            this.imageList1.Images.SetKeyName(591, "591.png");
            this.imageList1.Images.SetKeyName(592, "592.png");
            this.imageList1.Images.SetKeyName(593, "593.png");
            this.imageList1.Images.SetKeyName(594, "594.png");
            this.imageList1.Images.SetKeyName(595, "595.png");
            this.imageList1.Images.SetKeyName(596, "596.png");
            this.imageList1.Images.SetKeyName(597, "597.png");
            this.imageList1.Images.SetKeyName(598, "598.png");
            this.imageList1.Images.SetKeyName(599, "599.png");
            this.imageList1.Images.SetKeyName(600, "600.png");
            this.imageList1.Images.SetKeyName(601, "601.png");
            this.imageList1.Images.SetKeyName(602, "602.png");
            this.imageList1.Images.SetKeyName(603, "603.png");
            this.imageList1.Images.SetKeyName(604, "604.png");
            this.imageList1.Images.SetKeyName(605, "605.png");
            this.imageList1.Images.SetKeyName(606, "606.png");
            this.imageList1.Images.SetKeyName(607, "607.png");
            this.imageList1.Images.SetKeyName(608, "608.png");
            this.imageList1.Images.SetKeyName(609, "609.png");
            this.imageList1.Images.SetKeyName(610, "610.png");
            this.imageList1.Images.SetKeyName(611, "611.png");
            this.imageList1.Images.SetKeyName(612, "612.png");
            this.imageList1.Images.SetKeyName(613, "613.png");
            this.imageList1.Images.SetKeyName(614, "614.png");
            this.imageList1.Images.SetKeyName(615, "615.png");
            this.imageList1.Images.SetKeyName(616, "616.png");
            this.imageList1.Images.SetKeyName(617, "617.png");
            this.imageList1.Images.SetKeyName(618, "618.png");
            this.imageList1.Images.SetKeyName(619, "619.png");
            this.imageList1.Images.SetKeyName(620, "620.png");
            this.imageList1.Images.SetKeyName(621, "621.png");
            this.imageList1.Images.SetKeyName(622, "622.png");
            this.imageList1.Images.SetKeyName(623, "623.png");
            this.imageList1.Images.SetKeyName(624, "624.png");
            this.imageList1.Images.SetKeyName(625, "625.png");
            this.imageList1.Images.SetKeyName(626, "626.png");
            this.imageList1.Images.SetKeyName(627, "627.png");
            this.imageList1.Images.SetKeyName(628, "628.png");
            this.imageList1.Images.SetKeyName(629, "629.png");
            this.imageList1.Images.SetKeyName(630, "630.png");
            this.imageList1.Images.SetKeyName(631, "631.png");
            this.imageList1.Images.SetKeyName(632, "632.png");
            this.imageList1.Images.SetKeyName(633, "633.png");
            this.imageList1.Images.SetKeyName(634, "634.png");
            this.imageList1.Images.SetKeyName(635, "635.png");
            this.imageList1.Images.SetKeyName(636, "636.png");
            this.imageList1.Images.SetKeyName(637, "637.png");
            this.imageList1.Images.SetKeyName(638, "638.png");
            this.imageList1.Images.SetKeyName(639, "639.png");
            this.imageList1.Images.SetKeyName(640, "640.png");
            this.imageList1.Images.SetKeyName(641, "641.png");
            this.imageList1.Images.SetKeyName(642, "642.png");
            this.imageList1.Images.SetKeyName(643, "643.png");
            this.imageList1.Images.SetKeyName(644, "644.png");
            this.imageList1.Images.SetKeyName(645, "645.png");
            // 
            // tab_baseInfo
            // 
            this.tab_baseInfo.Controls.Add(this.tableLayoutPanel2);
            this.tab_baseInfo.Location = new System.Drawing.Point(4, 22);
            this.tab_baseInfo.Name = "tab_baseInfo";
            this.tab_baseInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tab_baseInfo.Size = new System.Drawing.Size(580, 288);
            this.tab_baseInfo.TabIndex = 0;
            this.tab_baseInfo.Text = "基本信息";
            this.tab_baseInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(574, 282);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 253F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(568, 276);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvlocalStConfig);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel4.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(247, 270);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "站位";
            // 
            // dgvlocalStConfig
            // 
            this.dgvlocalStConfig.AllowUserToAddRows = false;
            this.dgvlocalStConfig.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvlocalStConfig.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvlocalStConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvlocalStConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColStationNum,
            this.ColStationName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvlocalStConfig.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvlocalStConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvlocalStConfig.Location = new System.Drawing.Point(3, 17);
            this.dgvlocalStConfig.Margin = new System.Windows.Forms.Padding(2);
            this.dgvlocalStConfig.Name = "dgvlocalStConfig";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvlocalStConfig.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvlocalStConfig.RowHeadersVisible = false;
            this.dgvlocalStConfig.RowTemplate.Height = 30;
            this.dgvlocalStConfig.Size = new System.Drawing.Size(241, 250);
            this.dgvlocalStConfig.TabIndex = 0;
            this.dgvlocalStConfig.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvlocalStConfig_CellClick);
            this.dgvlocalStConfig.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvlocalStConfig_CellContentClick);
            this.dgvlocalStConfig.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvlocalStConfig_CellValidated);
            this.dgvlocalStConfig.Enter += new System.EventHandler(this.dgvlocalStConfig_Enter);
            this.dgvlocalStConfig.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvlocalStConfig_MouseClick);
            // 
            // ColStationNum
            // 
            this.ColStationNum.HeaderText = "站号";
            this.ColStationNum.Name = "ColStationNum";
            this.ColStationNum.Width = 140;
            // 
            // ColStationName
            // 
            this.ColStationName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColStationName.HeaderText = "站名";
            this.ColStationName.Name = "ColStationName";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvDevice);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(256, 3);
            this.groupBox2.Name = "groupBox2";
            this.tableLayoutPanel4.SetRowSpan(this.groupBox2, 2);
            this.groupBox2.Size = new System.Drawing.Size(309, 270);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "硬件接口";
            // 
            // dgvDevice
            // 
            this.dgvDevice.AllowUserToAddRows = false;
            this.dgvDevice.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column1});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDevice.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevice.Location = new System.Drawing.Point(3, 17);
            this.dgvDevice.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDevice.Name = "dgvDevice";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevice.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDevice.RowHeadersVisible = false;
            this.dgvDevice.RowTemplate.Height = 30;
            this.dgvDevice.Size = new System.Drawing.Size(303, 250);
            this.dgvDevice.TabIndex = 0;
            this.dgvDevice.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvDevice_CellBeginEdit);
            this.dgvDevice.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDevice_CellEndEdit);
            this.dgvDevice.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDevice_CellValidated);
            this.dgvDevice.Enter += new System.EventHandler(this.dgvDevice_Enter);
            this.dgvDevice.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvDevice_MouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "设备名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "类型";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "地址";
            this.Column1.Name = "Column1";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tab_baseInfo);
            this.tabControl1.Location = new System.Drawing.Point(0, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(588, 314);
            this.tabControl1.TabIndex = 0;
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.MenuStrip1.Name = "contextMenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // FrmStationConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 368);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_concel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmStationConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "工位配置";
            this.Load += new System.EventHandler(this.FrmStationConfig_Load);
            this.tab_baseInfo.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvlocalStConfig)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.MenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_concel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tab_baseInfo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvlocalStConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStationNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStationName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.ContextMenuStrip MenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
    }
}