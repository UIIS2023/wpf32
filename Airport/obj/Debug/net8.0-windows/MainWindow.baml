   M S B A M L   `   `   `  ����B  >Airport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:AirportAirport  5   	       Airport.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Airport     5   	     � Title$  
MainWindow��5   	   $	��500��6   $	��850��6)   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGridCenter��5
      $��Center=�6+   $��Center=�6F   $
��auto��6c   $
��auto��6p      
IsReadOnly$
 true��5         AlternatingRowBackground
 ��� �   CanUserAddRows$ False��6E      SelectionUnit$ FullRow=�6\      SelectionMode$ Single=�6t   � 5   
   $��Center=�6   $	��Top=�61   $
��auto��6I   $	��500��6W    � Orientation$ 
Horizontal=�6c   .+�5      �� -   5         ��Name$ 
btnAirline��5      $��Aviokompanije��6)   $
��Left=�6A   $	��Top=�6\   $��30��6t   $	��100��6�   6�   �� 5      -   5        $ btnAirplane��5      $��Avioni��6*   $��Bottom=�6;   $��30��6V   $��Right=�6b   $	��100��6~   6�   �� 5      -   5        $ 	btnFlight��5      $��Letovi��6(   $��Bottom=�69   $��30��6T   $��Right=�6`   $	��100��6|   6�   �� 5      -   5        $ btnFlightOperator��5      $��Operateri leta��60   $��Bottom=�6I   $��30��6d   $��Right=�6p   $	��100��6�   6�   �� 5      -   5        $ btnFlightTicket��5      $��Parovi: karta - let��6.   $��Bottom=�6L   $��30��6g   $��Right=�6s   $	��100��6�   6�   �� 5      -   5        $ btnGate��5      $
��Gejt��6&   $��Bottom=�65   $��30��6P   $��Right=�6\   $	��100��6x   6�   �� 5      -   5        $ btnPassenger��5      $��Putnici��6+   $��Bottom=�6=   $��30��6X   $��Right=�6d   $	��100��6�   6�   �� 5      -	   5        $ btnPilot��5      $��Piloti��6'   $��Bottom=�68   $��30��6S   $��Right=�6_   $	��100��6{   6�   �� 5      -
   5        $ 	btnRunway��5      $��Piste��6(   $��Bottom=�68   $��30��6S   $��Right=�6_   $	��100��6{   6�   �� 5      -   5        $ 	btnTicket��5      $��Karte��6(   $��Bottom=�68   $��30��6S   $��Right=�6_   $	��100��6{   6�   5      �� 5   
   $��Center=�6   $��Bottom=�62   $
��auto��6M   $
��auto��6[   	 �� Orientation$	 
Horizontal=�6h   .+�5      �� -   5        $ btnAdd��5      $��Dodaj��6%   $
��Left=�65   $	��Top=�6P   $��30��6h   $��80��6t   6   �� 5      -   5        $ 	btnChange��5      $��Izmeni��6(   $��Bottom=�69   $��30��6T   $��Right=�6`   $��80��6|   6�   �� 5      -   5        $ 	btnDelete��5      $��Obriši��6(   $��Bottom=�69   $��30��6T   $��Right=�6`   $��80��6|   6�   5      5      5      