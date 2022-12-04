#!/usr/bin/python
# -*- coding: UTF-8 -*-

file_path = 'metadataheader.ozs'
file=open(file_path,"w")
file.write("find_str_in_file Il2CppGlobalMetadataHeader 0 $head\n")

file.write("\n\n")

for i in range(1,60):#60*4=240 ,240<264 比最短的header少
    file.write("$h"+str(i)+" = $head\n")
    file.write("$h"+str(i)+" += "+str(i+1)+"\n")

file.write("\n\n")

for i in range(1,30):
    file.write("swap_line ")
    file.write("$h"+str(i)+" ")
    file.write("$h"+str(60-i)+"\n")
    
file.write("\n\n")

file.close()