import os
import cv2 as cv
files = os.listdir()
photo = ['.png', '.jpg', '.jpeg']
for file in files:
    if ".py" not in file:
        """
        frame = cv.imread(file, cv.IMREAD_GRAYSCALE)
        height = len(frame)
        width = len(frame[0])
        print(f"{height} + {width}")"""
        if not file.startswith('background'):
            os.rename(file, './background' + file)
