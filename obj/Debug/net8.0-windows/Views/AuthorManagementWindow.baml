   M S B A M L   `   `   `  ����  BookManager.    (BookManager.Views.AuthorManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Author Management��5   	   $	��600��6#   $	��800��60    � WindowStartupLocation$ CenterOwner=�5   	    � 	Resources 5         ModernButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers	 P� Property
 P� Value  DangerButton �� BasedOn��%%  &	        &	     7   �� 5   
   " ��6%   .�5	      ��  � �� �#4CAF506+   �� 5
       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 12,6q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$	 100��6)   �� 5       � ��$ SemiBold"�6+   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 3��5   !   #��z��5   !   #��z��5   !   ._�5      �� $��Center=�6/   $��Center=�5   -   5       5      P� 5      	 �@J�IsMouseOver$

 True��6=   . �5   "   ��  � �� �#45a0496?   5      P� 5      	 � {�$
 False��6;   . �5    "   ��  � �� �#cccccc6?   5!      5#      5%      5&      �� 5(   
   " ��6%   8   .�5)      ��  � �� �#f443366+   �� 5*       � �� 5+      �� 5,      " ��6&   .R�5-      �� #��z��6!   $ 3��5.   !   #��z��5/   !   #��z��50   !   ._�51      �� $��Center=�6/   $��Center=�52   /   53       54      P� 55      	 �@J�IsMouseOver$

 True��6=   . �56   "   ��  � �� �#da190b6?   57      P� 58      	 � {�$
 False��6;   . �59   "   ��  � �� �#cccccc6?   5:      5<      5>      5?      5@      .��5B      � ���#f5f5f5 � RowDefinitions 5C   
   �� 5D      $
��Auto �6   �� 5E      $��* �6   �� 5F      $
��Auto �6   �� 5G      $
��Auto �6   .+�5K   
   �� $��0��6   ���#2c3e50 �� Padding$ 20q�63   ._�5L      �� $��👥 Author Management��6   $��24(�5M      $
��Bold"�6&   
�������$��Center=�5N   +   5O      �� 5R   
   $��1��6   
�������$��20q�61   ���#ddd$��1q�5S   $   $ 5��68   ._�5T      �  5U      �� 5V      $
��Auto �6$   �� 5W      $��* �6$   .+�5Z      �� $��0��6   ���#34495e$ 15q�6;   ._�5[      �� $��Authors List��6    $��16(�5\       $
��Bold"�6.   
�������5]       5^      &   System.Windows.Controls.DataGrid  5`      -   5          Name$ AuthorsDataGrid��5`   (   $��1��6      AutoGenerateColumns$ False��5a         CanUserAddRows$ False��5b         GridLinesVisibility$ 
Horizontal=�5c         AlternatingRowBackground �#f9f9f9   Columns 5e      0  *System.Windows.Controls.DataGridTextColumn  5f      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6V      
IsReadOnly$
 True��6a   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *
AuthorId+6a     5g      $ 
First Name��6-   $ * 6_    6A   �� *	FirstName+6_     5h      $ 	Last Name��6-   $ * 6]    6@   �� *
LastName+6]     5i      $ Nationality��6-   $	 150 6b    6B   �� *Nationality+6b     5j      $ Created Date��6-   $	 120 6|   $
 True��6�    6C   �� *CreatedDate+ �� StringFormat$ 
yyyy-MM-dd��6�   5l      5m      5n      �� 5q   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Center=�5r      $��20q�62   .+�5s      �� -   5         ��Name$ AddAuthorButton��5s   ,   $��➕ Add Author��6   #��[  5t      �� 5u      -   5        $ UpdateAuthorButton��5u   0   $��💾 Update Author��6   #��[  5v      �� 5w      -   5        $ DeleteAuthorButton��5w   1   $��🗑️ Delete Author��6   #��[ 5x      �� 5y      -   5        $ RefreshButton��5y   *   $��🔄 Refresh��6   #��[  5z      5{      �� 5~   
   -   5        $ CloseButton��5~   .   $��3��6   $��Close��6   $��Center=�5      $��20q�6.   #��[  5�      5�      5�      