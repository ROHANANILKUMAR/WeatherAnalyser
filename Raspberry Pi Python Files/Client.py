import socket

class Client:
    s=socket.socket()
    ip=''
    port=''
    def __init__(this,ip,port):
        this.ip=ip
        this.port=port
        this.s.connect((ip,port))
    def Read(x):
        return s.recv(x).decode()
    def Send(msg):
        s.send(msg.encode(0)
    
