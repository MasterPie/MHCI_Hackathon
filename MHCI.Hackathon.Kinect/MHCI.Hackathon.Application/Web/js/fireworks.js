/**
 * Copyright (C) 2011 by Paul Lewis for CreativeJS. We love you all :)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

 var mainCanvas = document.createElement('canvas');
 var mainContext = mainCanvas.getContext('2d');
 var snapShot = null;

 function create () {
  var Fireworks = (function() {

    // declare variables
    var particles = [],
    // mainCanvas = null,
    // mainContext = null,
    indivCanvas = null,
    indivContext = null,
    fireworkCanvas = null,
    fireworkContext = null,
    viewportWidth = 0,
    viewportHeight = 0,
    playerTarget = null
    playerColor = null;

    var playerInfo = {
      player: null,
      craziness: null,
      volume: null
    };

    var updateValues = function (player, craziness, volume) {
      playerInfo.player = player;
      playerInfo.craziness = craziness;
      playerInfo.volume = volume;
    };

    /**
     * Create DOM elements
     */
    function initialize() {

      // start by measuring the viewport
      onWindowResize();

      // create a canvas for the fireworks
      indivCanvas = document.createElement('canvas');
      indivContext = indivCanvas.getContext('2d');    

      // off screen canvas buffer
      fireworkCanvas = document.createElement('canvas');
      fireworkContext = fireworkCanvas.getContext('2d');

      // set up the colours for the fireworks
      createFireworkPalette(12);

      // set the dimensions on the canvas
      setMainCanvasDimensions();

      // add the canvas in
      document.body.appendChild(mainCanvas);
      document.addEventListener('mouseup', createFirework, true);
      setInterval(function(){createFirework();},2000);

      if (playerInfo.player === 1) {
        indivCanvas.style.zIndex="4";
      } else if (playerInfo.player === 2) {
        indivCanvas.style.zIndex="3";
      } else if (playerInfo.player === 3) {
        indivCanvas.style.zIndex="2";
      } else if (playerInfo.player === 4) {
        indivCanvas.style.zIndex="1";
      }

      // and now we set off
      update();
    }

    /**
     * Pass through function to create a new firework
     */
    function createFirework() {
        if (playerInfo.player === 1) {
        playerColor = 192; //yellow
        } else if (playerInfo.player === 2) {
        playerColor = 396; //green
        } else if (playerInfo.player === 3) {
        playerColor = 756; //blue
        } else if (playerInfo.player === 4) {
        playerColor = 0; //red
      }

      playerTarget = (Math.random() * mainCanvas.height / 10) + ((10 - playerInfo.volume) * mainCanvas.height / 10);

      createParticle(null,playerTarget,null,playerColor,null);
    }

    /**
     * Creates a block of colours for the
     * fireworks to use as their colouring
     */
    function createFireworkPalette(gridSize) {

      var size = gridSize * 10;
      fireworkCanvas.width = size;
      fireworkCanvas.height = size;
      fireworkContext.globalCompositeOperation = 'source-over';

      // create 100 blocks which cycle through
      // the rainbow... HSL is teh r0xx0rz
      for(var c = 0; c < 100; c++) {

        var marker = (c * gridSize);
        var gridX = marker % size;
        var gridY = Math.floor(marker / size) * gridSize;

        fireworkContext.fillStyle = "hsl(" + Math.round(c * 3.6) + ",100%,60%)";
        fireworkContext.fillRect(gridX, gridY, gridSize, gridSize);
        fireworkContext.drawImage(
          Library.bigGlow,
          gridX,
          gridY);
      }
    }

    /**
     * Update the canvas based on the
     * detected viewport size
     */
    function setMainCanvasDimensions() {
      indivCanvas.width = viewportWidth;
      indivCanvas.height = viewportHeight;

      mainCanvas.width = viewportWidth;
      mainCanvas.height = viewportHeight;
    }

    /**
     * The main loop where everything happens
     */
    function update() {
      // mainContext.putImageData(snapShot,0,0);
      clearContext();
      requestAnimFrame(update);
      drawFireworks();
    }

    /**
     * Clears out the canvas with semi transparent black. 
     */
    function clearContext() {
      mainContext.fillStyle = "rgba(0,0,0,0.2)";
      mainContext.fillRect(0, 0, viewportWidth, viewportHeight);
    }

    /**
     * Passes over all particles particles
     * and draws them
     */
    function drawFireworks() {
      var a = particles.length;

      while(a--) {
        var firework = particles[a];

        // if the update comes back as true
        // then our firework should explode
        if(firework.update()) {

          // kill off the firework, replace it
          // with the particles for the exploded version
          particles.splice(a, 1);

          // if the firework isn't using physics
          // then we know we can safely(!) explode it... yeah.
          if(!firework.usePhysics) {

          FireworkExplosions.circle(firework, playerInfo, Fireworks);

          }
        }

        // pass the canvas context and the firework
        // colours to the
        firework.render(indivContext, fireworkCanvas);

        snapShot = indivContext.getImageData(0,0,indivCanvas.width,indivCanvas.height);
        console.log(snapShot);
      }
    }

    /**
     * Creates a new particle / firework
     */
    function createParticle(pos, target, vel, color, usePhysics) {

      pos = pos || {};
      target = target || {};
      vel = vel || {};

      particles.push(
        new Particle(
          // position
          {
            x: pos.x || Math.floor(Math.random()*(viewportWidth-100)) + 100,
            y: pos.y || viewportHeight + 10
          },

          {
            y: target
          },        

          // velocity
          {
            x: vel.x || Math.random() * 3 - 1.5,
            y: vel.y || 0
          },

          color,

          usePhysics)
      );
    }

    /**
     * Callback for window resizing -
     * sets the viewport dimensions
     */
    function onWindowResize() {
      viewportWidth = window.innerWidth;
      viewportHeight = window.innerHeight;
    }

    // declare an API
    return {
      initialize: initialize,
      createParticle: createParticle,
        update: updateValues
    };

  })();

   return Fireworks;
 }

/**
 * Represents a single point, so the firework being fired up
 * into the air, or a point in the exploded firework
 */
var Particle = function(pos, target, vel, marker, usePhysics) {

  // properties for animation
  // and colouring
  this.GRAVITY  = 0.06;
  this.alpha    = 1;
  this.easing   = Math.random() * 0.02;
  this.fade     = Math.random() * 0.1;
  this.gridX    = marker % 120;
  this.gridY    = Math.floor(marker / 120) * 12;
  this.color    = marker;

  this.pos = {
    x: pos.x || 0,
    y: pos.y || 0
  };

  this.vel = {
    x: vel.x || 0,
    y: vel.y || 0
  };

  this.lastPos = {
    x: this.pos.x,
    y: this.pos.y
  };

  this.target = {
    y: target.y || 0
  };

  this.usePhysics = usePhysics || false;

};

/**
 * Functions that we'd rather like to be
 * available to all our particles, such
 * as updating and rendering
 */
Particle.prototype = {

  update: function() {

    this.lastPos.x = this.pos.x;
    this.lastPos.y = this.pos.y;

    if(this.usePhysics) {
      this.vel.y += this.GRAVITY;
      this.pos.y += this.vel.y;

      // since this value will drop below
      // zero we'll occasionally see flicker,
      // ... just like in real life! Woo! xD
      this.alpha -= this.fade;
    } else {

      var distance = (this.target.y - this.pos.y);

      // ease the position
      this.pos.y += distance * (0.03 + this.easing);

      // cap to 1
      this.alpha = Math.min(distance * distance * 0.00005, 1);
    }

    this.pos.x += this.vel.x;

    return (this.alpha < 0.005);
  },

  render: function(context, fireworkCanvas) {

    var x = Math.round(this.pos.x),
        y = Math.round(this.pos.y),
        xVel = (x - this.lastPos.x) * -5,
        yVel = (y - this.lastPos.y) * -5;

    context.save();
    context.globalCompositeOperation = 'lighter';
    context.globalAlpha = Math.random() * this.alpha;

    // draw the line from where we were to where
    // we are now
    context.fillStyle = "rgba(255,255,255,0.3)";
    context.beginPath();
    context.moveTo(this.pos.x, this.pos.y);
    context.lineTo(this.pos.x + 1.5, this.pos.y);
    context.lineTo(this.pos.x + xVel, this.pos.y + yVel);
    context.lineTo(this.pos.x - 1.5, this.pos.y);
    context.closePath();
    context.fill();

    // draw in the images
    context.drawImage(fireworkCanvas,
      this.gridX, this.gridY, 12, 12,
      x - 6, y - 6, 12, 12);
    context.drawImage(Library.smallGlow, x - 3, y - 3);

    context.restore();
  }

};

/**
 * Stores references to the images that
 * we want to reference later on
 */
var Library = {
  bigGlow: document.getElementById('big-glow'),
  smallGlow: document.getElementById('small-glow')
};

/**
 * Stores a collection of functions that
 * we can use for the firework explosions. Always
 * takes a firework (Particle) as its parameter
 */
var FireworkExplosions = {

  /**
   * Explodes in a roughly circular fashion
   */
  circle: function(firework, playerInfo, Fireworks) {

    var count = 100;
    var angle = (Math.PI * 2) / count;
    while(count--) {

      var randomVelocity = 4 + Math.random() * 4;
      var particleAngle = count * angle;

      var playerVelocity = 1 + playerInfo.craziness*(7/10);

      Fireworks.createParticle(
        firework.pos,
        null,
        {
          x: Math.cos(particleAngle) * playerVelocity,
          y: Math.sin(particleAngle) * playerVelocity
        },
        firework.color,
        true);
    }
  }
};

var a, b, c, d;

// Go
window.onload = function() {
   a = create();
   a.update(1, 5, 2);
   a.initialize();

   b = create();
   b.update(2, 5, 4);
   b.initialize();

   c = create();
   c.update(3, 5, 6);
   c.initialize();

   d = create();
   d.update(4, 5, 8);
   d.initialize();
};

function acceptAction(playerNum, craziness, volume) {
    if (playerNum == 1)
        a.update(1, craziness, volume);
    else if (playerNum == 2)
        b.update(2, craziness, volume);
    else if (playerNum == 3)
        c.update(3, craziness, volume);
    else if (playerNum == 4)
        d.update(4, craziness, volume);
}

