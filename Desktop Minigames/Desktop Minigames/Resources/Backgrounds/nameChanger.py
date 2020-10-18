import os
import cv2 as cv

image_ext = ['png', 'jpg', 'jpeg']
allowed_ratios = [16, 9, 16, 10, 4, 3]
min_photo_width = 1500
idx = 0

def get_file_extension(file):
	return str(file).split('.')[-1]

def is_valid_photo(photo):
	img = cv.imread(file, cv.IMREAD_GRAYSCALE)
	width =len(img[0]) 
	height = len(img)
	if (width < min_photo_width):
		return False
	global allowed_ratios
	i = 0
	while i < len(allowed_ratios):
		if width / height == allowed_ratios[i] / allowed_ratios[i + 1]:
			return True
		i = i + 2

files = os.listdir()
for file in files:
	if get_file_extension(file) in image_ext:
		if is_valid_photo(file):
			while True:
				try:
					os.rename(file, f"./background{idx}.{get_file_extension(file)}")
					break
				except:    
					idx = idx + 1
		else:
			os.remove(file)













