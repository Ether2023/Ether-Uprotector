#!/usr/bin/python
# -*- coding: UTF-8 -*-

file_path = 'metadataheader.ozs'
file=open(file_path,"w")
file.write("find_str_in_file Il2CppGlobalMetadataHeader 0 $head\n")

file.write("printd $head\n")
file.write("prints \\n")

file.write("\n\n")

#获取行数变量
for i in range(1,60):#60*4=240 ,240<264 比最短的header少
    file.write("$h"+str(i)+" = $head\n")
    file.write("$h"+str(i)+" += "+str(1+i)+"\n\n")
    file.write("read_file_int32_lines metadata_header_repl.txt "+str(i)+" $s"+str(i)+"\n")
    file.write("$s"+str(i)+" += $head\n")
    file.write("$s"+str(i)+" += 2\n\n")

file.write("\n\n")

#Header换行
for i in range(1,60):
    file.write("swap_line ")
    file.write("$h"+str(i)+" ")
    file.write("$s"+str(i)+"\n\n")
    
    file.write("prints swap_\n")
    file.write("printd $h"+str(i)+"\n")
    file.write("prints _\n")
    file.write("printd $s"+str(i)+"\n")
    file.write("prints \\n\n\n")
    
file.write("\n\n")

file.close()

#Generate repl file
# file=open("metadata_header_repl.txt","w")
# for i in range(1,60):
    # file.write(str(60-i)+"\n")


