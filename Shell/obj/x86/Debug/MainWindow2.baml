   M S B A M L   `   `   `  ����  WPF_hl_17xy_cn     WPF_hl_17xy_cn.MainWindow2   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)    � WindowStartupLocation$ CenterScreen=�65   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5
   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $	��Top=�6;   .+�5      �� -   5        $��_ctlBtnLogin��5   "   $��登录��6   66   �� 5      -   5        $��btnAutoAuction��5   (   $��自动购买带星球员��6   6>   �� 5      -   5        $��_ctlTextCount��5      5      �� 5   
   $��1��6   .@�5      �� $	��Log��6   .��5      �� -   5        $��_ctlEditLog��5   ,    �� 
IsReadOnly . " �� VerticalScrollBarVisibility$ Visible=�6?   5      �� 5      $��Players��6   .��5      &   System.Windows.Controls.DataGrid  -   5        $��_ctlGridPlayers��5      5      5      5      5      