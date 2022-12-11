#!/usr/bin/python
# -*- coding: UTF-8 -*-

file_path = 'metadataheader.ozs'
file=open(file_path,"w")
file.write("insert_from_file 10 front_head.h\n\n")
file.write("find_str_in_file Il2CppGlobalMetadataHeader 0 $head\n")

file.write("printd $head\n")
file.write("prints \\n")

file.write("\n\n")

#获取行数变量
for i in range(1,30):#60*4=240 ,240<264 比最短的header少
    file.write("read_file_int32_lines metadata_header_repl.txt "+str(i)+" $h"+str(i)+"\n")
    file.write("$h"+str(i)+" += $head\n")
    file.write("$h"+str(i)+" += 1\n\n")
    file.write("read_file_int32_lines metadata_header_repl.txt "+str(i+30)+" $s"+str(i)+"\n")
    file.write("$s"+str(i)+" += $head\n")
    file.write("$s"+str(i)+" += 1\n\n")

file.write("\n\n")

#Header换行
for i in range(1,30):
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
file=open("metadata_header_repl.txt","w")
#格式:1-30行是替换左值,31-60行是替换右值
#即1和31换,2和32,以此类推
#下面是一个生成示例,顺序可用自定义
#建议直接手动修改metadata_header_repl.txt
for i in range(1,30):
    file.write(str(i)+"\n")
#空一行
file.write("\n")
for i in range(1,30):
    file.write(str(60-i)+"\n")


