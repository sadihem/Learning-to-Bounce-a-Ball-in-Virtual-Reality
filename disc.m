 
This is simply a function which returns the discretized values of the results obtained from the
simulation.m file.
function [ Sxballhit , Sballvx , Sballvy ]= disc ( xballhit , ballvx , ballvy)
rightlimit = 10;
leftlimit = 0;
Sballvy = 7;
Sxballhit = 7;
if 0.5 > xballhit && xballhit >=0
fake = 1;
Sxballhit = 5;
elseif 1.5 > xballhit && xballhit >= 0.5
fake = 1;
Sxballhit = 4;
elseif 2.5 > xballhit && xballhit >= 1.5
fake = 1;
Sxballhit = 3;
elseif 3.5 > xballhit && xballhit >= 2.5
fake = 1;
Sxballhit = 2;
elseif 4.5 > xballhit && xballhit >= 3.5
fake = 1;
Sxballhit = 1;
elseif 5.5 > xballhit && xballhit >= 4.5
fake = 1;
Sxballhit = 6;
elseif 6.5 > xballhit && xballhit >= 5.5
fake = 1;
Sxballhit = 1;
elseif 7.5 > xballhit && xballhit >= 6.5
fake = 1;
Sxballhit = 2;
elseif 8.5 > xballhit && xballhit >= 7.5
fake = 1;
Sxballhit = 3;
elseif 9.5 > xballhit && xballhit >= 8.5
fake = 1;
Sxballhit = 4;
elseif 10 > xballhit && xballhit >= 9.5
fake = 1;
Sxballhit = 5;
end
% discreticize ballvx
if xballhit >=( leftlimit + rightlimit )/ 2;
if ballvx <- 4.5
Sballvx = 11;
elseif - 4.5 <= ballvx && ballvx <- 3.5
Sballvx = 10;
elseif - 3.5 <= ballvx && ballvx <- 2.5
Sballvx = 9;
elseif - 2.5 <= ballvx && ballvx <- 1.5
Sballvx = 8;
elseif - 1.5 <= ballvx && ballvx <-.5
Sballvx = 7;
elseif -. 5 <= ballvx && ballvx <.5
Sballvx = 6;
elseif . 5 <= ballvx && ballvx < 1.5
Sballvx = 5;
elseif 1.5 <= ballvx && ballvx < 2.5
Sballvx = 4;
elseif 2.5 <= ballvx && ballvx < 3.5
Sballvx = 3;
elseif 3.5 <= ballvx && ballvx < 4.5
Sballvx = 2;
elseif 4.5 < ballvx || ballvx == 4.5;
Sballvx = 1;
end
end
if xballhit <( leftlimit + rightlimit )/ 2;
if ballvx <- 4.5
Sballvx = 1;
elseif - 4.5 <= ballvx && ballvx <- 3.5
Sballvx = 2;
elseif - 3.5 <= ballvx && ballvx <- 2.5
Sballvx = 3;
elseif - 2.5 <= ballvx && ballvx <- 1.5
Sballvx = 4;
elseif - 1.5 <= ballvx && ballvx <-.5
Sballvx = 5;
elseif -. 5 <= ballvx && ballvx <.5
Sballvx = 6;
elseif . 5 <= ballvx && ballvx < 1.5
Sballvx = 7;
elseif 1.5 <= ballvx && ballvx < 2.5
Sballvx = 8;
elseif 2.5 <= ballvx && ballvx < 3.5
Sballvx = 9;
elseif 3.5 <= ballvx && ballvx < 4.5
Sballvx = 10;
elseif 4.5 < ballvx || ballvx == 4.5;
Sballvx = 11;
end
end
% discreticize ballvy
% if ballvy > 0 || ballvy ==0
% disp ( 'error')
if -. 5 < ballvy && ballvy < 0 || ballvy ==- 0.5;
Sballvy = 6;
elseif - 1.5 < ballvy && ballvy <-. 5 || ballvy ==- 1.5;
Sballvy = 5;
elseif - 2.5 < ballvy && ballvy <- 1.5 || ballvy ==- 2.5;
Sballvy = 4;
elseif - 3.5 < ballvy && ballvy <- 2.5 || ballvy ==- 3.5;
Sballvy = 3;
elseif - 4.5 < ballvy && ballvy <- 3.5 || ballvy ==- 4.5;
Sballvy = 2;
elseif ballvy <- 4.5
Sballvy = 1;
end,end
The code for Q- Learning is provided in below.
