import ServerConnect
import socket
from multiprocessing.dummy import Pool
import serial
import time
import AnalyserCore as ac
import sys

f=open("SerialNo.txt",'r')
f.close()
ip=open('localip.txt','r')
ReadData=ip.read().split(';')
ip.close()

localip=ReadData[0]
print(localip)
port=int(ReadData[2])
print(port)
b=None
s=None
ar=serial.Serial('/dev/ttyACM0',9600)

"""def TryBkdr():
    global b
    b=socket.socket()
    b.conect(('192.168.43.5',bkdrport))
    print('backdoor app connected')
    """

def TryConnect():
    global s
    s=socket.socket()
    s.connect((localip,port))
    


ac.decode_list=['ldr','temp','humid','rain','gas','man','rainbool','flood','mansig']

#Arduino Sensor Vals
LDRVal=0
TempVal=0
HumidityVal=0
Gas=0
Man=False
RainVal=0
RainBool=False
Flood=0
ManSig=0

sensor_data=dict()

def Read():
    data=s.recv(10)
    return data.decode()


def Send(msg):
    s.send(msg.encode())


def ServerSide():
    try:
        TryConnect()
        ar.write('LON'.encode())
        while True:
            read=Read()
            if(read!=''):
                print(read,"from server")
            if(read=='ser'):
                Send(ser)
                print('Request for serial number...')
            elif(read=='senval'):
                print('Request for sensor values...')
                Send("dta<{0},{1},{2},{3},{4},{5},{6},{7},{8}>".format(LDRVal,TempVal,HumidityVal,RainVal,Gas,Man,RainBool,Flood,ManSig))
            elif(read=='Closing'):
                print("Closing")
                ar.write('LOF'.encode())
                sys.exit(0)
            elif(read=='Alive'):
                Send('true')

    except:
        ar.write('LOF'.encode())
        sys.exit(0)




def GetSensorReadings():
    global sensor_data
    global LDRVal
    global TempVal
    global HumidityVal
    global RainVal
    global Man
    global Gas
    global RainBool
    global Flood
    global ManSig
    #print("requesting data")
    ar.write("dta".encode())
    time.sleep(.1)
    read=ar.readline().decode()
    #print("Data recieved: ",read)
    if('<' in read and '>' in read and ',' in read):
        sensor_data=ac.decode(read)
        print(sensor_data)
        LDRVal=sensor_data['ldr']
        TempVal=sensor_data['temp']
        HumidityVal=sensor_data['humid']
        RainVal=sensor_data['rain']
        Man=sensor_data['man']
        Gas=sensor_data['gas']
        RainBool=sensor_data['rainbool']
        Flood=sensor_data['flood']
        ManSig=sensor_data['mansig']

Err=False

def ArduinoSide():
    global Err
    while True:
        GetSensorReadings()
        if(sensor_data!={}):
            response=ac.check(sensor_data)
            if("Err:" in response):
                Err=True
                Send(response)
                #time.sleep(100)
                #ar.write("RON".encode())
            else:
                if(Err):
                    Err=False
                    Send("EndEmer")
                    #time.sleep(100)
                    #ar.write("ROF".encode())
            
        
        

def Run():
    
    pool_Ser=Pool(processes=1)
    pool_Ser.apply_async(ServerSide)

    #pool_bkdr=Pool(processes=1)
    #pool_bkdr.apply_async(TryBkdr)
    
    #pool_Ar=Pool(processes=1)
    #pool_Ar.apply_async(ArduinoSide)
    print('Analyser running')
    time.sleep(2)
    ArduinoSide()
    
    
    
Run()
