import socket
import time
import os
import RPi.GPIO as g

led=2



g.setmode(g.BCM)
g.setwarnings(False)
g.setup(led,g.OUT)
g.output(led,g.LOW)
s = socket.socket()          
f=open("SerialNo.txt",'r')
ser=f.read()
f.close()
print("sending ",ser)
ip=open('localip.txt','r')
ReadData=ip.read().split(';')
ip.close()

localip=ReadData[0]
port=int(ReadData[2])
print(ReadData)
print(localip,port)

def connect():
    global s
    s = socket.socket()  
    s.connect((localip, port))
    s.send(ser.encode())

connect()
print("Trying to connect")
while True:
    try:
        read=s.recv(1024).decode()
        if(read=="Ok"):
            print("Connected")
            s.close()
            #os.system("python Analyser.py")
            #os.system("python BackDoor.py")
            g.output(led,g.HIGH)
            break
        else:
            s.close()
            connect()
    except:
        connect()
    
