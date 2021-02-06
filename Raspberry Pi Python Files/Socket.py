import socket
import time
'''
s=socket.socket()

port=8888

s.bind(('',port))

s.listen(5)

while True:
    c=s.accept()[0]
    c.send('Hello DuDe'.encode())

    import socket                
  '''
# Create a socket object 
s = socket.socket()          
  
# Define the port on which you want to connect 
port = 8888                
  
# connect to the server on local computer 
s.connect(('192.168.43.70', port)) 
time.sleep(3)
# receive data from the server 
# close the connection 
s.close()  
