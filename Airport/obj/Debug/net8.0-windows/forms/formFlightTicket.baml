   M S B A M L   `   `   `  ����B  >Airport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Airport.formsAirport.forms  5   	   $    Airport.forms.formFlightTicket   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Airport.forms     5   	     � Title$  formFlightTicket��5   	   $	��200��6"   $	��300��6/   .��5	      � .+�5
   
   �� $��Putnik:��6   $
��Left=�6"   $��28��6=   $��	10,10,0,0q�6I   $	��Top=�6\   �� 5   
   $��Karta:��6   $
��Left=�6!   $��28��6<   $��	10,40,0,0q�6H   $	��Top=�6[   �� 5   
   $
��Let:��6   $
��Left=�6   $��28��6:   $��	10,70,0,0q�6F   $	��Top=�6Y   �� 5   
   -   5         ��Name$ cmbPassenger��5      $��Right=�6)   $��	0,12,20,0q�6E   $	��Top=�6X   $	��160��6p    �� DisplayMemberPath$ 	Passenger��6|    �� SelectedValuePath$ passengerID��6�   �� 5   
   -   5        $ 	cmbTicket��5      $��Right=�6&   $��	0,42,20,0q�6B   $	��Top=�6U   $	��160��6m   $ flightNumber��6y   $ ticketID��6�   �� 5   
   -   5        $ 	cmbFlight��5      $��Right=�6&   $��	0,72,20,0q�6B   $	��Top=�6U   $	��160��6m   $ Flight��6y   $ flightID��6�   �� 5   
   -   5         ��Name$ btnSave��5      $
��Save��6"   $
��Left=�61   $��	28,0,0,10q�6L   $��Bottom=�6_   $��30��6z   $��80��6�   6�   �� 5   
   -   5        $ 	btnCancel��5      $��Cancel��6$   $��	0,0,28,10q�65   $��Bottom=�6H   $��30��6c   $��Right=�6o   $��80��6�   6�   5      5      