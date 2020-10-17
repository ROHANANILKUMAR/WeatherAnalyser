import socket
from multiprocessing.dummy import Pool
import time
import RPi.GPIO as g
import serial as sr
import AnalyserCore as ac
import os
from multiprocessing.dummy import Pool

ar=sr.Serial("/dev/ttyACM0",9600)

GLed=2

g.setmode(g.BCM)
g.setwarnings(False)
g.setup(GLed,g.OUT)

f=open("SerialNo.txt",'r')
ser=f.read()
f.close()
ip=open('localip.txt','r')
ReadData=ip.read().split(';')
ip.close()

localip=ReadData[1]
port=int(ReadData[3])

s=None
c=None

'''
b=None
bCl=None
def GetClient_bkdr():
    print("Waiting for client")
    global b
    bCl,addr=s.accept()
    print(addr)

def SetServer_bkdr():
    global b
    b=socket.socket()
    b.bind(('192.168.43.5',bkdrport))
    b.listen(1)
    GetClient_bkdr()
    '''

def GetClient():
    print("Waiting for client")
    global c
    c,addr=s.accept()
    print(addr)

def SetServer():
    global s
    s=socket.socket()
    s.bind((localip,port))
    s.listen(1)
    

def Read(s):
    data=s.recv(100)
    return data.decode()

def Send(msg):
    global c
    c.send(msg.encode())

def StartAnalyser():
    os.system("python Analyser.py")

def Do(read):
    if(read=='greenoff'):
        ar.write('LOF'.encode())
    elif(read=='greenon'):
        ar.write('LON'.encode())
    elif(read.startswith('exec')):
        try:
            exec(read.split(' ')[1])
        except:
            print("Error in exec command")
    elif(read=="dumpinfo"):
        ar.write('dta'.encode())
        time.sleep(.1)
        data=ar.readline().decode()#.replace("\n","").replace('\r','')
        Send(data)
        print(data)
    elif("ser" in read):
        f=open("SerialNo.txt","w")
        f.write(read.split(':')[1])
        f.close()
    elif(read=="start"):
        pool=Pool(processes=1)
        pool.apply_async(StartAnalyser)
    

def RunServer():
    print('Back door connected')
    while True:
        try:
            read=Read(c)
            print(read)
            if(read=="rec"):
                print("Reconnect")
                Setup()
                break
            elif(read!=''):
                print("msg=",read)
                Do(read)
        except:
            Setup()
        
def Setup():
    GetClient()
    RunServer()  


#Client_Pool=Pool()
#Client_Pool.apply_async(SetServer_bkdr)
SetServer()
Setup()

 
