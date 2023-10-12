using UnityEngine;

public static class MovePatterns
{
    public static void MoveOnPlatform(EnemyController ec)
    {
        //Variables to make calculations easier: left and right sides
        float xDistance = (ec._stats._rayXOffset * ec._spriteSize.x);
        Vector3 leftOffset = Vector2.left * xDistance;
        Vector3 rightOffset = Vector2.right * xDistance;

        //Variables to make calculations easier: bottom left and right sides
        float yDistance = (ec._stats._rayYOffset * ec._spriteSize.y);
        Vector3 downRightOffset = (Vector3.down * yDistance) + rightOffset;
        Vector3 downLeftOffset = (Vector3.down * yDistance) + leftOffset;


        //Raycast to check left side and right side of sprite
        RaycastHit2D hitLeftSide = Physics2D.Raycast
                     (ec.transform.position + leftOffset, Vector2.left,
                     xDistance, ec._stats._groundCheck);

        RaycastHit2D hitRightSide = Physics2D.Raycast
                     (ec.transform.position + rightOffset, Vector2.right,
                     xDistance, ec._stats._groundCheck);

        //Raycast to check bottom left side and bottom right side of sprite       
        RaycastHit2D hitdownRight = Physics2D.Raycast
                     (ec.transform.position + downRightOffset, Vector2.down, 1f,
                      ec._stats._groundCheck);

        RaycastHit2D hitdownLeft = Physics2D.Raycast
                     (ec.transform.position + downLeftOffset, Vector2.down, 1f,
                      ec._stats._groundCheck);
        
        //if enemy is moving
        if (ec._currentSpeed != 0)
        {
            //if enemy is moving and is about to hit something on either side
            if (hitLeftSide.collider !=null ^ hitRightSide.collider != null)
            {
                //reverse direction
                ec._movement.x *= -1f;
            }
            //otherwise, if enemy’s about to fall on left side AND downRight hits something
            else if (hitdownLeft.collider == null ^ hitdownRight.collider == null)
            {
                //reverse direction
                ec._movement.x *= -1f;
            }
        }
    } 


    public static void Jump(EnemyController ec)
    {
        /*using a conditional operator
        we check if movement in the positive y direction already exists
        this toggles jumping on and off every call to this method*/
        ec._movement.y = (ec._movement.y == 0f) ? 1f : 0f;
    }

    public static void CircleAroundPoint(EnemyController ec)
    {
        //convert the angle from degrees to radians using a built-in conversion
        float radians = ec._angle * Mathf.Deg2Rad;

        /* To move in a Vector direction a radius distance away from the center point, 
         * determine based on angle the X and Y position
         * the sprite needs to travel to follow 
         * the circumference of the circle, using sin and cos
         */
        float xPosition = ec._spawnLocation.x + (ec._radius * Mathf.Cos(radians));
        float yPosition = ec._spawnLocation.y + (ec._radius * Mathf.Sin(radians));

        ec.transform.position = new Vector3(xPosition, yPosition, 0f);

        //Gradually change the angle based on the speed of the sprite
        ec._angle += ec._movement.x * ec._currentSpeed * Time.deltaTime;
        //keep angle value around 360 degrees using remainder %
        ec._angle %= 360f;
    }
}
