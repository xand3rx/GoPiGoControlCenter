import requests
import signalr
import gopigo
import sys
import atexit
import picamera
import datetime
import time

# globals
camera = picamera.PiCamera()
url = 'http://gopigo.azurewebsites.net/Photo/Upload'

def takePicture():
    camera.resolution = (1024, 768)
    camera.start_preview()
    now = datetime.datetime.now().isoformat()
    picture_name = '/home/pi/Desktop/pic_{}.jpg'.format(now)
    camera.capture(picture_name)
    camera.stop_preview()
    files = {'uploadPicture': open(picture_name, 'rb')}
    response = requests.post(url, files=files)
    if response.status_code == requests.codes.ok:
        print('file sent')
        print(response.text)
    else:
        print('request went bad')
        print('status code is:', response.status_code)

def ledToggle():
    gopigo.led_on(0)
    gopigo.led_on(1)
    time.sleep(1)
    gopigo.led_off(0)
    gopigo.led_off(1)
    
def forward():
    gopigo.set_speed(200)
    gopigo.fwd()
    
def backward():
    gopigo.set_speed(200)
    gopigo.bwd()
    
def turnLeft():
    gopigo.set_speed(80)
    gopigo.left_rot()
    
def turnRight():
    gopigo.set_speed(80)
    gopigo.right_rot()

def justStop():
    gopigo.stop()

def executeCommand(command):
    command2execute = {
    'Forward': forward,
    'Left': turnLeft,
    'Right': turnRight,
    'Backward': backward,
    'Stop': justStop,
    'TakePicture': takePicture,
    'ToggleLeds': ledToggle,
    }
    try:
        command2execute[command]()
    except Exception as inst:
        print(type(inst), inst)

    print('Command executed: ', command)
    
#create error handler
def print_error(error):
    print('error: ', error)
    
def registerCar():
    try:
        goPiGoHub.server.invoke('registerCar')
    except Exception as inst:
        print(type(inst), inst)

def main():
    with requests.Session() as session:
        #create a connection etc.
        print("session started")
        connection = signalr.Connection("http://gopigo.azurewebsites.net/signalr", session)
        goPiGoHub = connection.register_hub('goPiGoHub')

        # send command to function
        goPiGoHub.client.on('executeCommand', executeCommand)

        # #create error handler
        def doStop():
            gopigo.stop()
            connection.close()
            session.close()

        atexit.register(doStop)
        
        def registerCar():
            try:
                goPiGoHub.server.invoke('registerCar')
            except Exception as inst:
                print(type(inst), inst)
            
        #process errors
        connection.error += print_error

        with connection:
            print("connection started")
            registerCar()
            print("connection to GoPiGo active...")
            print("awaiting commands from automation hub ...")
            while(1==1):
                connection.wait(10)
                
if __name__ == "__main__":
    main()