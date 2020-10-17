import time


def lowldrval(data):
    if(data['ldr']<100):
        if(data['rain']<1000):
            return "Side Effect of rain"
        elif(data['rain']>1000 and time.localtime().tm_hour>7 and time.localtime().tm_hour<13):
            return "Bad Weather"



def check(data):
    per=0
    msg=''
    """if(data['ldr']>100):
        print("Toomuchlight")
        return 'Err:Problem'"""
    if(data['temp']>50):
        if(data['ldr']>100):
            if(data['gas']>200):
                per=100
                msg='Raging fire'
            else:
                per=75
                msg='Fire'
        else:
            per=50
            msg='Possible fire'
    if(data['gas']>200):
        per=75
        msg='Gas Leak'
        
    if(data['rain']<900):
        if(data['flood']==1):
            if(data['ldr']<50):
                if(data['humid']>30):
                    per=100
                    msg='Heavy rain and flood'
                else:
                    per=75
                    msg='Rain and flood'
            else:
                per=75
                msg='Flood'
        else:
            per=25
            msg='Rain'
    if(data['flood']==1):
        per=100
        msg="Flood Alert"
    if(data['mansig']==1):
        per=100
        msg='Call for help'
    if(msg!='' and per!=0):
        return "Err:"+msg+":"+str(per)+":"+str(data['man'])
    else:
        return "No problem"
        

        
    
decode_list=[]

def decode(string):
    res_dict={}
    for s,d in zip(string.replace(">","").replace("<","").replace("\n","").replace("\r","").split(','),decode_list):
        res_dict[d]=int(s)
    return res_dict
