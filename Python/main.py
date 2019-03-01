import math
import numpy as np
import itertools
import time
class TaskDefinition:
    numberOfPhotos: int = 0
class SlideCompareDef:
    index: int = 0
    score: int = 0
    def __init__(self, index, score):
        self.index = index
        self.score = score
class PhotoDefinition:
    id: int = 0
    orientation: str = ""
    tagsNumber: int = 0
    tags: list = []
    isUsed: bool= False
    photoScore:int=0

    def __init__(self, id,orientation, tagsNumber, tags):
        self.orientation = orientation
        self.id = id
        self.tagsNumber = tagsNumber
        self.tags = tags


def getFilesLines(filesNumber)-> []:
    filesDataLines = []
    statementsFolderPath = "..\\Statements\\"
    fileNames = [
        "a_example.txt",
        "b_lovely_landscapes.txt",
        "c_memorable_moments.txt",
        "d_pet_pictures.txt",
        "e_shiny_selfies.txt"
    ]
    for fileNumber in range(0, filesNumber):
        fileName = fileNames[fileNumber]
        with open(statementsFolderPath + fileName, 'r') as myfile:
            fileLines = myfile.readlines()
        filesDataLines.append(fileLines)
    return filesDataLines


def getTaskDefinitions(fileLines)-> TaskDefinition:
    taskDefinition = TaskDefinition()
    definitions = fileLines[0]
    fileLines.remove(fileLines[0])
    taskDefinitionData = definitions.split(" ")
    taskDefinition.numberOfPhotos = int(taskDefinitionData[0])
    return taskDefinition


def prepareResults(slideShow, fileIndex):
    file = open(f'{fileIndex}_result.txt', "w")
    file.writelines(f'{slideShow.__len__()} \n')
    for slidePhoto in slideShow:
        if(isinstance(slidePhoto, list)):
            file.writelines("{} {}\n".format(slidePhoto[0].id,
                                               slidePhoto[1].id))
               
        else:
            file.writelines("{}\n".format(slidePhoto.id))
    file.close()


def extractTagsFromSlide(slide):
    if (isinstance(slide, list)):
        return slide[0].tags + slide[1].tags
    else:
        return slide.tags  
    
def countSameTagsBetweenSlides(slide1, slide2):
    tags1 = extractTagsFromSlide(slide1)
    tags2 = extractTagsFromSlide(slide2)
    return len(list(set(tags1).intersection(tags2)))

def countDiffrence12(slide1, slide2):
    tags1 = extractTagsFromSlide(slide1)
    tags2 = extractTagsFromSlide(slide2)
    return len(list(set(tags2).difference(tags1)))

def countDiffrence21(slide1, slide2):
    tags1 = extractTagsFromSlide(slide1)
    tags2 = extractTagsFromSlide(slide2)
    return len(list(set(tags1).difference(tags2)))

def countScoreOfSlides(tags1, slide2):
    tags2 = set(extractTagsFromSlide(slide2))
    sameLen = len(set(tags1).intersection(tags2))
    dif1 =  len(tags1) -sameLen 
    dif2 = len(tags2) -sameLen 
    return min(sameLen, dif1, dif2)
    
def extractPhotos(fileLines):
    photos = []
    for index, line in enumerate(fileLines):
        photoLine = line.replace("\n", "").split(" ")
        photos.append(PhotoDefinition(index, photoLine[0], photoLine[1], photoLine[2:]))
    return photos

def getAllByType(photos, typeName)-> list:
    result = []
    for i in range(0, photos.__len__()):
        photo = photos[i]
        if (photo.orientation == typeName):
            result.append(photo)
    return result

def createSlidesFromVertical(photos):
    result = []
    verticalPhotos = getAllByType(photos, 'V')
    if (verticalPhotos.__len__() == 0):
        return result
    i = 0
    while (i < verticalPhotos.__len__()):
        result.append([verticalPhotos[i], verticalPhotos[i+1]])
        i = i + 2
    return result
def countTags(elem):
    return extractTagsFromSlide(elem).__len__()
fileDataLines = getFilesLines(5)
#minScore=10 e- max
minScore=4
#for fileIndex, fileLines in enumerate(fileDataLines):

fileIndex = 3
fileLines = fileDataLines[fileIndex]
print(f'File Index: {fileIndex}')
taskDefinition = getTaskDefinitions(fileLines)
print(f'{taskDefinition.numberOfPhotos}')
photos = extractPhotos(fileLines)
allHorizontal = getAllByType(photos,'H')
allVertical   = createSlidesFromVertical(photos)
#unpreparedSlides = allHorizontal + allVertical
unpreparedSlides = sorted(allHorizontal + allVertical,reverse=True, key = countTags)
slideShow = [unpreparedSlides[0]]

unpreparedSlides.pop(0)

start_time = time.time()
start_all_time = time.time()
print(f'Started {fileIndex} at {start_all_time}')
while (1):
    baseSlide = slideShow[-1]
    tags1 = extractTagsFromSlide(baseSlide)
    scores = []
    bestScore = 0
    bestSlide = None
    bestSlideIndex=0
    unprepLen = unpreparedSlides.__len__()
    if(unprepLen==0):
        break
    for slideIndex,slide in enumerate(unpreparedSlides):
        score= countScoreOfSlides(tags1, slide)
        if(score>bestScore or (bestSlide is None and slideIndex ==unprepLen-1)):
            bestScore = score
            bestSlide = slide
            bestSlideIndex = slideIndex
        if(bestScore>=minScore):
            break
    slideShow.append(bestSlide)
    unpreparedSlides.pop(bestSlideIndex)
    if(unprepLen%100 == 0):
        curTime = time.time()
        elapsed_time = curTime - start_time
        start_time = curTime
        print(f'Left slides: {unprepLen} elapsedTime: {elapsed_time}')
allTime =time.time()-  start_all_time 
print(f'Finished file {fileIndex} in {allTime}')
prepareResults(slideShow, fileIndex)