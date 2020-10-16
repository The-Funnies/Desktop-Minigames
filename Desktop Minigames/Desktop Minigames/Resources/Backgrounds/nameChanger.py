import os
files = os.listdir()
for file in files:
    if ".py" in file:
       os.rename(file, './background' + file)
print(os.listdir())