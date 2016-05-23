# WebSocket-StreamInsight
Real time Sensor Visualisation app data processing using StreamInsight
Objective: To establish real time processing of mobile device Sensor Visualisation app data using Streaminsight and websocket protocol.

Output : To indicate mobile device pointing in different directions as mobile device is rotated (as shown below).

*************************Output:******************************
Input Adapter Call
Received sensor data: 5/23/2016 6:10:56 AM +00:00 - Device is pointing towards South-East direction at 118.03125 deg
Received sensor data: 5/23/2016 6:10:56 AM +00:00 - Device is pointing towards South-East direction at 117.984375 deg
Received sensor data: 5/23/2016 6:10:58 AM +00:00 - Device is pointing towards East direction at 111.0625 deg
Received sensor data: 5/23/2016 6:10:58 AM +00:00 - Device is pointing towards East direction at 110.703125 deg
Received sensor data: 5/23/2016 6:10:58 AM +00:00 - Device is pointing towards East direction at 107.125 deg
Received sensor data: 5/23/2016 6:11:00 AM +00:00 - Device is pointing towards North-East direction at 65.765625 deg
Received sensor data: 5/23/2016 6:11:00 AM +00:00 - Device is pointing towards North-East direction at 62.09375 deg
Received sensor data: 5/23/2016 6:11:00 AM +00:00 - Device is pointing towards North-East direction at 61.796875 deg

***********************Steps*******************

1) Download the Sensor Visualisation app on your android mobile phone from the Playstore.
2) Ensure that your mobile device and your desktop/laptop are in the same network by creating a hotspot or connecting to the same WiFi network.
3) Once connected, check the IP address shown in the Sensor Visualisation app.
4) This solution fetches the data from app using WebSocket(WS) protocol. 
5) Download this solution (built using Visual Studio 2015) and change the mobile device IP Address value and the Port WS value in the App.config file as indicated on your device.
6) Thats it...Run the solution and Bingo!!!...You will see the above indicated output in console window as you rotate the mobile device in different directions.
