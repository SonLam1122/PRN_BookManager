   M S B A M L   `   `   `  ����  BookManager0    *BookManager.Views.CategoryManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Category Management��5   	   $	��600��6%   $	��800��62    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ 	CanResize=�5   	    � 	Resources 5         ModernButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius	 �� Triggers
 P� Property P� Value  DangerButton �� BasedOn �� Triggers��%�  &	        &	     7   �� 5	   
   " ��6%   .�5
      ��  � �� �#4CAF506+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 12,6q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$	 100��6)   �� 5       � ��$ SemiBold"�6+   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 3��5   !   #��z��5   !   #��z��5   !   ._�5      �� $��Center=�6/   $��Center=�5   -   5      	 5      P� 5      
 �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� �#45a0496?   5      P� 5       
 � {�$ False��6;   . �5!   "   ��  � �� �#cccccc6?   5"      5$      5&      5'      �� 5)   
   " ��6%   8   .�5*      ��  � �� �#f443366+    5+      P� 5,      
 �@J�IsMouseOver$
 True��61   . �5-      ��  � �� �#da190b63   5.      50      51      .��53      � ���#f5f5f5 � RowDefinitions 54   
   �� 55      $
��Auto �6   �� 56      $��* �6   �� 57      $
��Auto �6   �� 58      $
��Auto �6   .+�5<   
   �� $��0��6   ���#2c3e50 �� Padding$ 20q�63   ._�5=      �� $��Category Management��6   $��24(�5>      $
��Bold"�6&   
�������$��Center=�5?   +   5@      �� 5C   
   $��1��6   
�������$��20q�61   ���#ddd$��1q�5D   $   $ 5��68   ._�5E      �  5F      �� 5G      $
��Auto �6$   �� 5H      $��* �6$   .+�5K      �� $��0��6   ���#34495e$ 15q�6;   ._�5L      �� $��Categories List��6    $��16(�5M       $
��Bold"�6.   
�������5N       5O      &   System.Windows.Controls.DataGrid  5Q      -   5          Name$ CategoriesDataGrid��5Q   (   $��1��6      AutoGenerateColumns$ False��5R         CanUserAddRows$ False��5S         GridLinesVisibility$ 
Horizontal=�5T         AlternatingRowBackground �#f9f9f9   Columns 5V      0  *System.Windows.Controls.DataGridTextColumn  5W      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6X      
IsReadOnly$
 True��6c   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *
CategoryId+6c     5X      $ Category Name��6-   $ * 6e    6D   �� *CategoryName+6e     5Y      $ Description��6-   $ * 6b    6B   �� *Description+6b     5Z      $ Created Date��6-   $	 120 6|   $
 True��6�    6C   �� *CreatedDate+ �� StringFormat$ 
yyyy-MM-dd��6�   5\      5]      5^      �� 5a   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Center=�5b      $��20q�62   .+�5c      �� -   5         ��Name$ AddCategoryButton��5c   ,   $��Add Category��6   #��[  5d      �� 5e      -   5        $ UpdateCategoryButton��5e   /   $��Update Category��6   #��[  5f      �� 5g      -   5        $ DeleteCategoryButton��5g   /   $��Delete Category��6   #��[ 5h      �� 5i      -   5        $ RefreshButton��5i   '   $��Refresh��6   #��[  5j      5k      �� 5n   
   -   5        $ CloseButton��5n   .   $��3��6   $��Close��6   $��Center=�5o      $��20q�6.   #��[  5p      5q      5r      