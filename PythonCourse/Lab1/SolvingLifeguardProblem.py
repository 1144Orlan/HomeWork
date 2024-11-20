import math

# d1 = float(input("Введите кратчайшее расстояние между спасателем и кромкой воды, d1 (ярды): "))
# d2 = float(input("Введите кратчайшее расстояние от утопающего до берега, d2 (футы): "))
# h = float(input("Введите боковое смещение между спасателем и утопающим, h (ярды): "))
# v_sand = float(input("Введите скорость движения спасателя по песку, v_sand (мили в час): "))
# n = float(input("Введите коэффициент замедления спасателя при движении в воде, n: "))
# theta1 = float(input("Введите направление движения спасателя по песку, theta1 (градусы): "))
# d1 = d1 /3 /5280   
# d2 = d2 /5280   
# h  = h /3 /5280  
# theta1 = math.radians(theta1)

d1 = 8.0/3/5280   #ярды в мили
d2 = 10.0/5280    #футы в мили
h = 50.0/3/5280   #ярды в мили 
v_sand = 5.0      #Скорость, миль в час
n = 2.0   #коэффициент замедления в воде
theta1 = math.radians(39.413)

x = d1*math.tan(theta1)

L1 = math.sqrt(math.pow(x, 2) + math.pow(d1, 2))

L2 = math.sqrt(math.pow((h-x), 2) + math.pow(d2, 2))

#t = 1/v_sand*(L1+n*L2)
t = ((1/v_sand*(L1+n*L2))*60)*60 #to seconds

angle = int(math.degrees(theta1))  # Convert radians back to degrees and round to integer
time = round(t, 1)  # Round to 1 decimal place

print(f"Если спасатель начнёт движение под углом theta1, равным {angle} градусам," 
      f"он достигнет утопающего через {time} секунды.")