# Облако тегов

Для визуализации облака тегов подобного этому, нужно реализовать алгоритм раскладки слов.

![TagCloud](https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/Web_2.0_Map.svg/800px-Web_2.0_Map.svg.png)


## Задача 1
Реализовать в режиме TDD алгоритм раскладки.

В конструкторе задается позиция центра облака тегов.
Для простоты тут нет слов, а есть только размер ограничивающего прямоугольника для каждого слова rectangleSize. 
Нужно найти расположение каждого такого прямоугольника.

Некоторые требования к раскладке:

  * Форма итогового облака должна быть близка к кругу с центром в точке center.
  * Прямоугольники не должны пересекаться друг с другом.
  * Облако должно быть плотным, чем плотнее, тем лучше.

Требования к тестам:

  * Придерживаться паттерна AAA.

## Задача 2

Сделать визуализацию получившейся раскладки. Сгенерировать 2-3 раскладки с разными параметрами.

## Задача 3

При падении теста необходимо создавать изображение раскладки.


## Сгенерированные изображения

![add_rectangle_on_left](TagsCloudVisualization/TagsCloudPictures/PutNextRectangle_Should_AddSecondRectangleOnTheFirstLeft_WhenHeightOfFirstIsGreaterThanTheWidth.png)

![add_rectangle_on_top](TagsCloudVisualization/TagsCloudPictures/PutNextRectangle_Should_AddSecondRectangleOnTheFirstTop_WhenWidthOfFirstIsGreaterThanTheHeight.png)

![add_rectangle_on_the_first_bottom](TagsCloudVisualization/TagsCloudPictures/PutNextRectangle_Should_AddThirdRectangleOnTheFirstBottom_WhenTheSecondIsOnTheFirstTop.png)

![put_rectangles_around](TagsCloudVisualization/TagsCloudPictures/PutNextRectangle_Should_PutBigRectanglesAroundSmallOneInCenter.png)

![add_rectangle_in_hole](TagsCloudVisualization/TagsCloudPictures/PutNextRectangle_Should_AddRectangleInHole_WhenThereIsASuitableHole.png)