import socket
import sys

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
# Bind the socket to the port
server_address = ('127.0.0.1', 13000)
print('starting up on %s port %s' % server_address)
sock.connect(server_address)
while(True):
    message = input("Enter the message: ")
    if message == "q":
        print('closing socket')
        sock.close()
        break
    else:
        print('sending "%s"' % message)
        sock.sendto(message.encode(), server_address)

        # Look for the response
        amount_received = 0
        amount_expected = len(message)

        while amount_received < amount_expected:
            data = sock.recv(16)
            amount_received += len(data)
            print('received "%s"' % data)
