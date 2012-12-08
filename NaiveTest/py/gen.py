bat_prefix = '..\\batch\\'
bat_suffix = '.bat'

exe_path = '..\\bin\\Release\\'

des = '.\\txt\\'

total = 1000;
for i in range(10):
    f = open(bat_prefix + str(i) + bat_suffix, 'w')
    f.write(exe_path + 'NaiveTest.exe ' + str(total) + ' ' + str(i) + ' > ' + des + str(i) +'.txt')
    f.close()
