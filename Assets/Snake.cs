using System.Collections.Generic; //so we can use List
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right; 
    // private Vector2Int input;
    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    public int initialSize = 4;

    // Start is called before the first frame update
    void Start()
    {
        ResetState();
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow turning up or down while moving in the x-axis
        // if (_direction.x != 0f)
        // {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                _direction = Vector2Int.up;
            } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                _direction = Vector2Int.down;
            }
        // }
        // Only allow turning left or right while moving in the y-axis
        // else if (_direction.y != 0f)
        // {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                _direction = Vector2Int.right;
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                _direction = Vector2Int.left;
            }
        // }
    }

    private void FixedUpdate()
    {
        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = _segments.Count - 1; i > 0; i--) {
            _segments[i].position = _segments[i - 1].position;
        }

        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
        int x = Mathf.RoundToInt(transform.position.x) + (int)_direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + (int)_direction.y;
        transform.position = new Vector2(x, y);
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        //_segments[_segments.Count - 1] this gets us the last one
        //we want to add onto the end of the snake, not the beginning
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    public void ResetState()
    {
        _direction = Vector2Int.right;
        transform.position = Vector3.zero;

        // Start at 1 to skip destroying the head 
        for (int i = 1; i < _segments.Count; i++) {
            Destroy(_segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        _segments.Clear();
        _segments.Add(this.transform);

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++) {
            Grow();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        } else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("SnakeSegment"))
        {
            ResetState();
        }
    }

}
