
% clear all
clc
tic
runs = 100000;
action1num = 5;
action2num = 5;
action3num = 5;
state1num = 7;
state2num = 11;
state3num = 7;
gamma = 0.8;
alpha = 0.5;
lastaction1 = 0;
lastaction2 = 0;
lastaction3 = 0;
lstate1 = 0;
lstate2 = 0;
lstate3 = 0;
global ballvylimit
ballvylimit =-. 5;
Aruns = zeros ( 10000 );
firstTenR = zeros ( 10000 );
runNumb = 0;
intI = 1;
runSums = 10;
totalRunsCnt = 0;
xballhit = 20;
Q = zeros ( action1num , action2num , action3num , state1num , state2num , state3num );
previousxHit = - 1;
for run = 1 : runs;
if mod ( run , runSums ) == 0
Aruns ( intI ) = totalRunsCnt / runSums;
totalRunsCnt = 0;
intI = intI + 1;
end
%%%%%%%%%%%%%% give initial values to states and actions
%% xballhit = rand ( 1 )* 10;
xballhit = 3 + rand ( 1 )* 6;
ballvx1 = rand ( 1 )* 3;
ballvx2 = rand ( 1 )*- 3;
% ballvx1 = rand ( 1 )* 7;
% ballvx2 = rand ( 1 )*- 7;
if abs ( ballvx1 )> abs ( ballvx2)
ballvx = ballvx1;
else
ballvx = ballvx2;
end
ballvx;
% ballvy = rand ( 1 )*- 5;
ballvy =- 1 + rand ( 1 )*- 2;
% get the states for the initial conditions
stepnum = 10 ; % how many steps in the episode
reward = 0;
% find the greedy action or choose randomly
for step = 1 : stepnum
[ Sxballhit0 , Sballvx0 , Sballvy0 ]= disc ( xballhit , ballvx , ballvy );
if xballhit >= 0 && xballhit <= 10 && ballvy < ballvylimit
totalRunsCnt = totalRunsCnt + 1;
actionsForGivenState = Q (:,:,:, Sxballhit0 , Sballvx0 , Sballvy0 );
% 1 dementionalize and find max
M = actionsForGivenState ;%= randn ( 10 , 10 , 10 , 10 );
[ C , I ] = max ( M (:));
temp = find ( max ( M (:)) == M (:));
[ C , I ] = max ( M (:));
C = C;
randI = temp ( randi ([ 1 length ( temp )]));
M ( randI );
[ I1 , I2 , I3 ] = ind2sub ( size ( M ), randI );
M ( I1 , I2 , I3 );
maxAction1 = I1;
maxAction2 = I2;
maxAction3 = I3;
optimalActionValue = C;
[ reward , nextxballhit , ballvxnext , ballvynext ] = simulation ( xballhit , ballvx , ballvy , I1 , I2 , I3 );
%
% if ( previousxHit == - 1)
% PlotHit ( xballhit , xballhit , nextxballhit , I1 , I2 , I3 , ballvxnext , 0)
% else
% PlotHit ( previousxHit , xballhit , nextxballhit , I1 , I2 , I3 , ballvxnext , ballvx )
% end
if ( runNumb < 10000)
runNumb = runNumb + 1;
firstTenR ( runNumb ) = reward;
end
lastaction1 = I1;
lastaction2 = I2;
lastaction3 = I3;
% nextxballhit , ballvxnext , ballvynext
% PlotHit ( previousPadX , currentPadX , nextPadX , PadAngle , PaddleXVelocity , PaddleYVelocity , vyoutput
, vyinput)
[ Sxballhit , Sballvx , Sballvy ]= disc ( nextxballhit , ballvxnext , ballvynext );
lstate1 = Sxballhit0;
lstate2 = Sballvx0;
lstate3 = Sballvy0;
actionsForGivenState = Q (:,:,:, Sxballhit , Sballvx , Sballvy );
% 1 dementionalize and find max
M = actionsForGivenState ;%= randn ( 10 , 10 , 10 , 10 );
[ C , I ] = max ( M (:));
temp = find ( max ( M (:)) == M (:));
[ C , I ] = max ( M (:));
C = C;
randI = temp ( randi ([ 1 length ( temp )]));
M ( randI );
[ I1 , I2 , I3 ] = ind2sub ( size ( M ), randI );
M ( I1 , I2 , I3 );
NextmaxAction1 = I1;
NextmaxAction2 = I2;
NextmaxAction3 = I3;
optimalActionValue = C;
delta = reward + gamma * Q ( NextmaxAction1 , NextmaxAction2 , NextmaxAction3 , Sxballhit , Sballvx ,
Sballvy )- Q ( maxAction1 , maxAction2 , maxAction3 , Sxballhit0 , Sballvx0 , Sballvy0 );
% E ( row , col , optimalA ) = E ( row , col , optimalA ) + 1;
% E ( row , col , optimalA ) = ( 1 - alpha )* E ( row , col , optimalA ) + 1
Q ( I1 , I2 , I3 , Sxballhit0 , Sballvx0 , Sballvy0 )= Q ( I1 , I2 , I3 , Sxballhit0 , Sballvx0 , Sballvy0 )+ alpha * delta;
AAA =[ nextxballhit , ballvxnext , ballvynext ];
previousxHit = xballhit;
xballhit = AAA ( 1 );
ballvx = AAA ( 2 );
ballvy = AAA ( 3 );
end
end
end
figure
x =( 1 : 10000 );
plot ( x , Aruns ( 1 : 10000 ))
xlabel ( '500 Runs')
ylabel ( 'Total Runs')
title ( '50,000 15 Hit Runs - Smart PVs')
figure
x =( 1 : 10000 );
plot ( x , firstTenR ( 1 : 10000 ))
xlabel ( 'Hit')
ylabel ( 'Reward')
title ( 'First 10,000 Rewards')
toc
% disp ( Q ( lastaction1 , lastaction2 , lastaction3 , lstate1 , lstate2 , lstate3 ));
