   M S B A M L   `   `   `  ����A  =Caesar, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:CaesarCaesar  5   	       Caesar.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Caesar     5   	   -   5          � Title$  
MainWindow��5   	   $	��850��6   $
��1400��6,   $	��850��6<   $
��1400��6I   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��1163 �6   �� 5      $��* �6   6+   .+�5   
   �� -   5        $��myCanvas��5   +   $
��1163��6   $	��819��6   
���� �6�   � 5   
   $��1��6    � RowDefinitions 5      �� 5      $
��auto �6    �� 5      .+�5      �� .+�5      �� -   5        $
��year��5      $��5q�6$   $��	Castellar)�6/   $��16(�6F   $��2q�6T   $��Center=�6`   �� 5      -   5        $
��gold��5      $��5q�6$   $��	Castellar)�6/   $��16(�6F   $��2q�6T   $��Center=�6`   �� 5      -   5        $
��food��5      $��5q�6$   $��	Castellar)�6/   $��16(�6F   $��2q�6T   $��Center=�6`   �� 5      -   5        $	��man��5      $��5q�6#   $��	Castellar)�6.   $��16(�6E   $��2q�6S   $��Center=�6_   �� 5      -   5        $��house��5      $��5q�6&   $��	Castellar)�61   $��16(�6H   $��2q�6V   .��6v   House6}   �� 5      -   5        $
��farm��5      $��5q�6%   $��	Castellar)�60   $��16(�6G   $��2q�6U   .��6t   Farm6z   �� 5      -	   5        $��storage��5      $��5q�6(   $��	Castellar)�63   $��16(�6J   $��2q�6X   .��6z   	Storage6�   �� 5!      -
   5        $��	draw_well��5!      $��5q�6*   $��	Castellar)�65   $��16(�6L   $��2q�6Z   .��6~   	Draw-well6�   �� 5#      -   5        $
��road��5#      $��5q�6%   $��	Castellar)�60   $��16(�6G   $��2q�6U   .��6t   Road6z   5$      �� 5%      $��1��6   $��Hidden=�6(   .��5&      �� -   5        $��Output��5&      $��Bottom=�6*    �� IsHyphenationEnabled . $	��150��6a   $	��150��6q   $��	Castellar)�6�   5'      5(      5)      5*      