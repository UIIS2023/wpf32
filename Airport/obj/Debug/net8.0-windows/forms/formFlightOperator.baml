   M S B A M L   `   `   `  ����B  >Airport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Airport.formsAirport.forms  5   	   &     Airport.forms.formFlightOperator   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Airport.forms     5   	     � Title$  formFlightOperator��5   	   $	��200��6$   $	��300��61   .��5	      � .+�5
   
   �� $��Pilot:��6   $
��Left=�6!   $��28��6<   $��	10,10,0,0q�6H   $	��Top=�6[   �� 5   
   $��Aviokompanija:��6   $
��Left=�6)   $��28��6D   $��	10,40,0,0q�6P   $	��Top=�6c   �� 5   
   $��Avion:��6   $
��Left=�6!   $��28��6<   $��	10,70,0,0q�6H   $	��Top=�6[   �� 5   
   -   5         ��Name$ cmbPilot��5      $��Right=�6%   $��	0,12,20,0q�6A   $	��Top=�6T   $	��160��6l    �� DisplayMemberPath$ Pilot��6x    �� SelectedValuePath$ pilotID��6�   �� 5   
   -   5        $ 
cmbAirline��5      $��Right=�6'   $��	0,42,20,0q�6C   $	��Top=�6V   $	��160��6n   $
 name��6z   $ 	airlineID��6�   �� 5   
   -   5        $ cmbAirplane��5      $��Right=�6(   $��	0,72,20,0q�6D   $	��Top=�6W   $	��160��6o   $ Airplane��6{   $ 
airplaneID��6�   �� 5   
   -   5         ��Name$ btnSave��5      $
��Save��6"   $
��Left=�61   $��	28,0,0,10q�6L   $��Bottom=�6_   $��30��6z   $��80��6�   6�   �� 5   
   -   5        $ 	btnCancel��5      $��Cancel��6$   $��	0,0,28,10q�65   $��Bottom=�6H   $��30��6c   $��Right=�6o   $��80��6�   6�   5      5      