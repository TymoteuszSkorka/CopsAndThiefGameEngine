import socket
import sys
import json
import numpy as np
import random


class Moves:
    def __init__(self, role, kClock, numOfMoves):
        self.m_sRole = role
        self.m_16Moves = np.empty((kClock, numOfMoves), dtype='int16')
        self.clearMoves()
    
    def clearMoves(self):
        self.m_16Moves[:, :] = 0
    

ClientSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

name = None
error_flag = False

server_address = ('127.0.0.1', 13000)
print('starting up on %s port %s' % server_address)
ClientSocket.connect(server_address)


MsgFromServer_hello = ClientSocket.recv(1024)
print(MsgFromServer_hello)
messageFromClient_HS = input(">")
ClientSocket.sendto(messageFromClient_HS.encode(), server_address)

MsgFromServer_state = ClientSocket.recv(1024)

while str(MsgFromServer_state) == "b'already taken or input error, pick again:'":
    print(MsgFromServer_state)
    messageFromClient_HS = input(">")
    ClientSocket.sendto(messageFromClient_HS.encode(), server_address)
    MsgFromServer_state = ClientSocket.recv(1024)

name = messageFromClient_HS
print(MsgFromServer_state)
#ustawienia ogolne ozgrywki
settings_json = ClientSocket.recv(1024)
print(settings_json)
settings = json.loads(settings_json)

if messageFromClient_HS == "T":
    myMoves = Moves(messageFromClient_HS, settings["kClock"], 1)
elif messageFromClient_HS == "P":
    myMoves = Moves(messageFromClient_HS, settings["kClock"], settings["numOfCops"])

#wysylamy wiadomosc ze chcemy juz zaczac
messageFromClient_GO = input("Type whateve if u'r ready: ")
ClientSocket.sendto(messageFromClient_GO.encode(), server_address)
#czekanie na potwierdzenie serwera ze gra sie zaczela
rolled_board_json = ClientSocket.recv(1024)
print(rolled_board_json)
rolled_board = json.loads(rolled_board_json)

while True:
    #tutaj wstawic do jsona tablice z naszymi ruchami ktora wyslemy do servera
    for i in range(len(myMoves.m_16Moves)):
        for j in range(len(myMoves.m_16Moves[i])):
            myMoves.m_16Moves[i, j] = random.randint(0, 5)
    myMoves.m_16Moves = myMoves.m_16Moves.tolist()
    messageFromClient = json.dumps(myMoves, default=lambda o: o.__dict__)
    myMoves.m_16Moves = np.asarray(myMoves.m_16Moves)
    myMoves.clearMoves()

    ClientSocket.sendto(messageFromClient.encode(), server_address)
    # MsgFromServer = ClientSocket.recv(2000)
    # print("Server answer: " + str(MsgFromServer))
    MsgFromServer_moves = ClientSocket.recv(2000)
    print(str(MsgFromServer_moves))

