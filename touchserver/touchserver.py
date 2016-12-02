import serial
import socket
import time

UDP_IP = "127.0.0.1"
UDP_PORT = 11123

comportName="COM1"

for line in open("comport-data.config"):
    comportName=line
    
sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP

while True: 
    try:                    
        with serial.Serial(comportName, 9600,timeout=5) as ser:
            while True:
                ser.write("p");
                line = ser.readline()   # read a '\n' terminated line
                sock.sendto(line,(UDP_IP,UDP_PORT))
                time.sleep(0.01)
    except:
        print "reopen port"
    
