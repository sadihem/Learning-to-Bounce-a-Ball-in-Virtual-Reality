
function [reward,nextxballhit,ballvxnext,ballvynext] = simulation( xballhit,ballvx,ballvy,i,j,k) %,padangel,padvx,padvy these are actions that
shoud not be in the code
rightlimit=10;
leftlimit = 0;
global ballvylimit
ballvylimit = 4;
g = 9.81;
ballnormal = 0.8;
padnormal = 0.8;
padtangent = 0.1;
balltangent = 0.8;
ballangle = atan2 ( ballvy , ballvx );
% padangel =[- 20 ,- 10 , 0 , 10 , 20 ]; in degrees
if xballhit >=( leftlimit + rightlimit )/2
Padangel =[ 0 , 10 , 20 , 30 , 40 ]* pi / 180;
%i
else
Padangel =-[ 0 , 10 , 20 , 30 , 40 ]* pi / 180 ; %i
end
if xballhit >=( leftlimit + rightlimit )/ 2;
Padvx =[- 2 ,- 1 , 0 , 1 , 2 ]; %j
else
Padvx =[ 2 , 1 , 0 ,- 1 ,- 2 ];
end
Padvy =[- 2 ,- 1 , 0 , 1 , 2 ]; %k
if xballhit >( leftlimit + rightlimit )/2
R =[ cos ( Padangel ( i )) sin ( Padangel ( i )); - sin ( Padangel ( i )) cos ( Padangel ( i )) ];
padvx = Padvx ( j );
padvy = Padvy ( k );
gooo = R *[ padvx ; padvy ];
padvt = gooo ( 1 ); padvn = gooo ( 2 );
fooo = R *[ ballvx ; ballvy ];
ballvt = fooo ( 1 ); ballvn = fooo ( 2 );
ballvtAFTER = balltangent * ballvt + padtangent * padvt;
ballvnAFTER =-( ballvn * ballnormal )+ padvn * padnormal;
Rinverse =[ cos ( Padangel ( i )) - sin ( Padangel ( i )); sin ( Padangel ( i )) cos ( Padangel ( i )) ];
hooo = Rinverse *[ ballvtAFTER ; ballvnAFTER ];
ballvxAFTER = hooo ( 1 );
ballvyAFTER = hooo ( 2 );% this is the velocity of ball right after impact
time_on_air = 2 * ballvyAFTER / g;
nextxballhit = xballhit + ballvxAFTER * time_on_air;
ballvxnext = ballvxAFTER;
ballvynext =- ballvyAFTER;
end
if xballhit <=( leftlimit + rightlimit )/2
R =[ cos ( Padangel ( i )) sin ( Padangel ( i )); - sin ( Padangel ( i )) cos ( Padangel ( i )) ];
padvx = Padvx ( j );
padvy = Padvy ( k );
gooo = R *[ padvx ; padvy ];
padvt = gooo ( 1 ); padvn = gooo ( 2 );
fooo = R *[ ballvx ; ballvy ];
ballvt = fooo ( 1 ); ballvn = fooo ( 2 );
ballvtAFTER = balltangent * ballvt + padtangent * padvt;
ballvnAFTER =-( ballvn * ballnormal )+ padvn * padnormal;
Rinverse =[ cos ( Padangel ( i )) - sin ( Padangel ( i )); sin ( Padangel ( i )) cos ( Padangel ( i )) ];
hooo = Rinverse *[ ballvtAFTER ; ballvnAFTER ];
ballvxAFTER = hooo ( 1 );
ballvyAFTER = hooo ( 2 );% this is the velocity of ball right after impact
time_on_air = 2 * ballvyAFTER / g;
nextxballhit = xballhit + ballvxAFTER * time_on_air;
ballvxnext = ballvxAFTER;
ballvynext =- ballvyAFTER;
end
if time_on_air <= 0.1
nextxballhit = rightlimit + 1;
% reward =- 1;
end
if ( nextxballhit > rightlimit ) || ( nextxballhit < leftlimit ) || ( ballvylimit < ballvynext )
reward =- 1;
else
reward = 1;
end
end
